
using Moq;
using System.Text;
using Newtonsoft.Json;
using KLG.Backend.Promotion.Models.Request;
using KLG.Backend.Promotion.Services.Business.Promotion;
using KLG.Backend.Promotion.Services.Configuration;
using KLG.Backend.Promotion.Services.Controllers.RestApi;
using KLG.Backend.Promotion.Services.Entities;
using KLG.Backend.Promotion.Services.Resources;

namespace KLG.Backend.Promotion.Tests.Helper
{
    public class SetupTesting
    {
        private readonly PromoSetup _promoSetup;

        public SetupTesting()
        {
            //Setup Promo
            _promoSetup = new PromoSetup();
            _promoSetup.RefreshWorkflow(GetWorkflow());
        }

        public async Task<Mock<Library.Microservice.DataAccess.IKLGDbProvider<DefaultDbContext>>> GetDbContextMoq()
        {
            Mock<Library.Microservice.DataAccess.IKLGDbProvider<DefaultDbContext>> dbContextMoq = new();

            return dbContextMoq;
        }

        public async Task<Mock<Library.Microservice.Configuration.IKLGConfiguration>> GetConfigMoq()
        {
            Mock<Library.Microservice.Configuration.IKLGConfiguration> configMoq = new();

            return configMoq;
        }

        public async Task<Mock<Library.Microservice.Messaging.IKLGMessagingProvider>> GetMessageMoq()
        {
            Mock<Library.Microservice.Messaging.IKLGMessagingProvider> messageMoq = new();

            return messageMoq;
        }

        public async Task<Mock<Serilog.ILogger>> GetLoggerMoq()
        {
            Mock<Serilog.ILogger> loggerMoq = new();

            return loggerMoq;
        }

        public async Task<Mock<IPromoSetup>> GetPromotionSetupMoq(PromoRequestDto promoRequestDto)
        {
            var resultPromoSetup = await _promoSetup.GetPromo(promoRequestDto.PromoType, promoRequestDto);

            Mock<IPromoSetup> promoSetupMoq = new();
            promoSetupMoq.Setup(q => q.GetPromo(promoRequestDto.PromoType, promoRequestDto)).ReturnsAsync(resultPromoSetup);

            return promoSetupMoq;
        }

        //public async Task<Mock<IPromoManager>> GetPromotionManagerMoq(PromoRequestDto promoRequestDto)
        //{
        //    PromoManager promoManager = new(promoSetupMoq.Object, dbContextMoq.Object);
        //    var resultPromoManagerFind = await promoManager.FindPromo(promoRequestDto);
        //    var resultPromoManagerValidate = await promoManager.ValidatePromo(promoRequestDto);

        //}

        public async Task<PromoController> GetPromoController(PromoRequestDto promoRequestDto)
        {
            //Setup KLG Cotr Base
            Mock<Library.Microservice.DataAccess.IKLGDbProvider<DefaultDbContext>> dbContextMoq = new();
            Mock<Library.Microservice.Configuration.IKLGConfiguration> configMoq = new();
            Mock<Library.Microservice.Messaging.IKLGMessagingProvider> messageMoq = new();
            Mock<Serilog.ILogger> loggerMoq = new();

            //Setup Promo Library
            var resultPromoSetup = await _promoSetup.GetPromo(promoRequestDto.PromoType, promoRequestDto);
            
            Mock<IPromoSetup> promoSetupMoq = new();
            promoSetupMoq.Setup(q => q.GetPromo(promoRequestDto.PromoType, promoRequestDto)).ReturnsAsync(resultPromoSetup);

            //Setup Promo Manager
            PromoManager promoManager = new(promoSetupMoq.Object, dbContextMoq.Object);
            var resultPromoManagerFind = await promoManager.FindPromo(promoRequestDto);
            var resultPromoManagerValidate = await promoManager.ValidatePromo(promoRequestDto);
            //var resultPromoManagerCalculate = await promoManager.CalculatePromo(promoRequestDto);

            Mock<IPromoManager> promoManagerMoq = new();
            promoManagerMoq.Setup(q => q.FindPromo(promoRequestDto)).ReturnsAsync(resultPromoManagerFind);
            promoManagerMoq.Setup(q => q.ValidatePromo(promoRequestDto)).ReturnsAsync(resultPromoManagerValidate);
            //promoManagerMoq.Setup(q => q.CalculatePromo(promoRequestDto, null)).ReturnsAsync(resultPromoManagerCalculate);

            //Setup Controller
            PromoController promoController = new(promoManagerMoq.Object, dbContextMoq.Object, configMoq.Object, messageMoq.Object, 
                                                loggerMoq.Object);

            return promoController;
        }

        private static string[] GetWorkflow()
        {
            DbTesting myDb = new();
            List<string> promoWorkflowList = new();

            List<PromoWorkflow> listPromoWorkflow = myDb.listmspromoworkflow;

            //Return Out Jika Tidak Terdapat Promo Workflow
            if (listPromoWorkflow.Count < 1) {
                return promoWorkflowList.ToArray();
            }

            foreach (var promoWorkflowHeader in listPromoWorkflow) {

                //Add Global Params
                List<GlobalParam> listGlobalParams = new();

                if (promoWorkflowHeader.PromoWorkflowExpression.Count > 0) {

                    foreach (var loopGlobalParams in promoWorkflowHeader.PromoWorkflowExpression) {
                        GlobalParam GlobalParams = new()
                        {
                            Name = loopGlobalParams.Code,
                            Expression = loopGlobalParams.Expression
                        };

                        listGlobalParams.Add(GlobalParams);
                    }

                }

                //get data rule table
                List<PromoRule> listPromoRule = listPromoWorkflow.FirstOrDefault(q => q.Id == promoWorkflowHeader.Id)
                                                .PromoRule.ToList();

                //Next Loop Jika Promo Rule Tidak di Temukan
                if (listPromoRule.Count < 1) {
                    continue;
                }

                //Add Rule 
                List<Rule> listRules = new();

                foreach (var loopRules in listPromoRule) {
                    StringBuilder ruleExp = new();
                    int groupLine = 0, countGroupLine = 0;

                    List<LocalParam> listLocalParams = new();

                    //Add Local Params
                    //Add Local Params Variable
                    if (loopRules.PromoRuleVariable.Count > 0) {
                        foreach (var loopLocalParamsVar in loopRules.PromoRuleVariable) {
                            LocalParam localParamas = new()
                            {
                                Name = loopLocalParamsVar.Code,
                                Expression = loopLocalParamsVar.ParamsExpression
                            };

                            listLocalParams.Add(localParamas);
                        }
                    }

                    //Add Local Params Expresion
                    if (loopRules.PromoRuleExpression.Count > 0) {
                        foreach (var loopLocalParamsExp in loopRules.PromoRuleExpression.OrderBy(q => q.Linenum)) {

                            LocalParam localParamas = new()
                            {
                                Name = loopLocalParamsExp.Code,
                                Expression = loopLocalParamsExp.ParamsExpression
                            };

                            listLocalParams.Add(localParamas);

                            var linkExp = loopLocalParamsExp.Linkexp != null &&
                                            loopLocalParamsExp.Linkexp != "" ? $" {loopLocalParamsExp.Linkexp} " : "";

                            if (loopLocalParamsExp.Groupline != 0) {
                                if (loopLocalParamsExp.Groupline > groupLine) {

                                    countGroupLine = loopRules.PromoRuleExpression
                                                        .Count(q => q.Groupline == loopLocalParamsExp.Groupline);
                                    ruleExp.Append("(" + loopLocalParamsExp.Code + linkExp);
                                    groupLine++;
                                    countGroupLine--;
                                } else {
                                    countGroupLine--;
                                    if (countGroupLine == 0) {
                                        ruleExp.Append(loopLocalParamsExp.Code + ")" + linkExp);
                                    } else {
                                        ruleExp.Append(loopLocalParamsExp.Code + linkExp);
                                    }
                                }
                            } else {
                                ruleExp.Append(loopLocalParamsExp.Code + linkExp);
                            }
                        }
                    }

                    PromoRule getDataResult = loopRules;
                    getDataResult.PromoRuleVariable = null;
                    getDataResult.PromoRuleExpression = null;
                    getDataResult.PromoWorkflow = null;

                    if(getDataResult.PromoRuleResult != null && getDataResult.PromoRuleResult.Count > 0) {
                        foreach (var loopResult in getDataResult.PromoRuleResult) {
                            loopResult.PromoRule = null;
                        }
                    }

                    Context addContext = new()
                    {
                        DataPromo = getDataResult
                    };

                    OnSuccess addOnsuccess = new()
                    {
                        Name = "ResultPromo",
                        Context = addContext
                    };

                    Actions addAction = new()
                    {
                        OnSuccess = addOnsuccess
                    };

                    Rule rules = new()
                    {
                        Actions = addAction,
                        RuleName = loopRules.Id,
                        SuccessEvent = Convert.ToString(loopRules.Cls) + "#" + Convert.ToString(loopRules.Lvl),
                        LocalParams = listLocalParams,
                        Expression = ruleExp.ToString(),
                    };

                    listRules.Add(rules);
                }

                //Add Workflow 
                RuleWorkflow ruleWorkflow = new()
                {
                    WorkflowName = promoWorkflowHeader.Id,
                    GlobalParams = listGlobalParams,
                    Rules = listRules
                };

                string convertToString = JsonConvert.SerializeObject(ruleWorkflow);
                promoWorkflowList.Add(convertToString);

            }

            return promoWorkflowList.ToArray();
        }
    }
}
