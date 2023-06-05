
using System.Text;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using KLG.Library.Microservice.Messaging;
using KLG.Library.Microservice.DataAccess;
using KLG.Library.Microservice.Configuration;
using KLG.Backend.Promotion.Services.Entities;
using KLG.Backend.Promotion.Services.Configuration;

namespace KLG.Backend.Promotion.Services.Business.Engine
{
    public class EngineManager : IEngineManager
    {
        private readonly IKLGDbProvider<DefaultDbContext> _dbProvider;
        private readonly IKLGMessagingProvider _messageProvider;
        private readonly IKLGConfiguration _configuration;

        public EngineManager(IKLGDbProvider<DefaultDbContext> dbProvider, IKLGMessagingProvider messageProvider,
            IKLGConfiguration configuration)
        {
            _dbProvider = dbProvider;
            _messageProvider = messageProvider;
            _configuration = configuration;
        }

        public async Task<string[]> GetWorkflow()
        {
            List<string> promoWorkflowList = new();

            List<PromoWorkflow> listPromoWorkflow = await _dbProvider.DbContext.PromoWorkflow
                .Where(q => q.ActiveFlag)
                .Include(q => q.PromoWorkflowExpression)
                .ToListAsync();

            if (listPromoWorkflow.Count > 0) {

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
                    List<PromoRule> listPromoRule = _dbProvider.DbContext.PromoRule
                        .Where(q => q.PromoworkflowId == promoWorkflowHeader.Id)
                        .Include(q => q.PromoRuleVariable)
                        .Include(q => q.PromoRuleExpression)
                        .Include(q => q.PromoRuleResult)
                        .ToList();

                    //Add Rule 
                    if (listPromoRule.Count > 0) {
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

                            foreach (var loopResult in getDataResult.PromoRuleResult) {
                                loopResult.PromoRule = null;
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

                }

            }

            return promoWorkflowList.ToArray();
        }
    }
}
