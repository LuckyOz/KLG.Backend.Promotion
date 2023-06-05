
using RulesEngine.Models;
using RulesEngine.Actions;

namespace KLG.Backend.Promotion.Services.Configuration
{
    public interface IPromoSetup
    {
        bool RefreshWorkflow(string[] workflowRules);
        Task<List<RuleResultTree>> GetPromo(string workflowPromo, object dataPromo);
    }

    public class PromoSetup : IPromoSetup
    {
        private RulesEngine.RulesEngine _rulesEngine;

        public bool RefreshWorkflow(string[] workflowRules)
        {
            var reSettings = new ReSettings
            {
                CustomActions = new Dictionary<string, Func<ActionBase>>{
                                          {"ResultPromo", () => new PromoResult()}
                                      }
            };

            _rulesEngine = new RulesEngine.RulesEngine(workflowRules, reSettings);

            return true;
        }

        public async Task<List<RuleResultTree>> GetPromo(string workflowPromo, object dataPromo)
        {
            var paramsPromo = new RuleParameter("paramsPromo", dataPromo);
            var resultList = await _rulesEngine.ExecuteAllRulesAsync(workflowPromo, paramsPromo);

            return resultList;
        }
    }
}
