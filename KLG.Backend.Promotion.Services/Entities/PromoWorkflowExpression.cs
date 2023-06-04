
using KLG.Library.Microservice.DataAccess;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KLG.Backend.Promotion.Services.Entities
{
    [Table("promo_workflow_expression")]
    public class PromoWorkflowExpression : KLGModelBase
    {
        [Column("promo_workflow_id"), MaxLength(40)]
        [ForeignKey("PromoWorkflow")]
        public string PromoworkflowId { get; set; }
        public PromoWorkflow PromoWorkflow { get; set; }

        [Column("code"), MaxLength(100)]
        public string Code { get; set; }

        [Column("name"), MaxLength(200)]
        public string Name { get; set; }

        [Column("expression"), DataType("text")]
        public string Expression { get; set; }
    }
}
