
using KLG.Backend.Promotion.Services.Entities;

namespace KLG.Backend.Promotion.Services.Configuration
{
    public class RuleWorkflow
    {
        public string WorkflowName { get; set; }
        public List<GlobalParam> GlobalParams { get; set; }
        public List<Rule> Rules { get; set; }
    }

    public class GlobalParam
    {
        public string Name { get; set; }
        public string Expression { get; set; }
    }

    public class Rule
    {
        public string RuleName { get; set; }
        public string Expression { get; set; }
        public string SuccessEvent { get; set; }
        public List<LocalParam> LocalParams { get; set; }
        public Actions Actions { get; set; }
    }

    public class LocalParam
    {
        public string Name { get; set; }
        public string Expression { get; set; }
    }

    public class Actions
    {
        public OnSuccess OnSuccess { get; set; }
    }

    public class OnSuccess
    {
        public string Name { get; set; }
        public Context Context { get; set; }
    }

    public class Context
    {
        public PromoRule DataPromo { get; set; }
    }
}
