
using KLG.Library.Microservice.DataAccess;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KLG.Backend.Promotion.Services.Entities
{
    [Table("promo_workflow")]
    public class PromoWorkflow : KLGModelBase
    {
        [Column("name"), MaxLength(200)]
        public string Name { get; set; }

        public ICollection<PromoWorkflowExpression> PromoWorkflowExpression { get; set; }

        public ICollection<PromoRule> PromoRule { get; set; }
    }
}
