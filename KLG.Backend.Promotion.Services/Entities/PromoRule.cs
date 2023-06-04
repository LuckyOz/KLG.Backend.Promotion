
using KLG.Library.Microservice.DataAccess;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KLG.Backend.Promotion.Services.Entities
{
    [Table("promo_rule")]
    public class PromoRule : KLGModelBase
    {
        [Column("promo_workflow_id"), MaxLength(40)]
        [ForeignKey("PromoWorkflow")]
        public string PromoworkflowId { get; set; }
        public PromoWorkflow PromoWorkflow { get; set; }

        [Column("name"), MaxLength(200)]
        public string Name { get; set; }

        [Column("cls")]
        public int Cls { get; set; }

        [Column("lvl")]
        public int Lvl { get; set; }

        [Column("item_type"), MaxLength(50)]
        public string ItemType { get; set; }

        [Column("result_type"), MaxLength(50)]
        public string ResultType { get; set; }

        [Column("promo_action_type"), MaxLength(100)]
        public string PromoActionType { get; set; }

        [Column("promo_action_value"), DataType("text")]
        public string PromoActionValue { get; set; }

        [Column("max_value"), MaxLength(200)]
        public string MaxValue { get; set; }

        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Column("end_date")]
        public DateTime EndDate { get; set; }

        public ICollection<PromoRuleVariable> PromoRuleVariable { get; set; }

        public ICollection<PromoRuleExpression> PromoRuleExpression { get; set; }

        public ICollection<PromoRuleResult> PromoRuleResult { get; set; }
    }
}
