
using KLG.Library.Microservice.DataAccess;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KLG.Backend.Promotion.Services.Entities
{
    [Table("promo_rule_variable")]
    public class PromoRuleVariable : KLGModelBase
    {
        [Column("promo_rule_id"), MaxLength(40)]
        [ForeignKey("PromoRule")]
        public string PromoruleId { get; set; }
        public PromoRule PromoRule { get; set; }

        [Column("linenum")]
        public int Linenum { get; set; }

        [Column("code"), MaxLength(100)]
        public string Code { get; set; }

        [Column("name"), MaxLength(200)]
        public string Name { get; set; }

        [Column("params_expression"), DataType("text")]
        public string ParamsExpression { get; set; }
    }
}
