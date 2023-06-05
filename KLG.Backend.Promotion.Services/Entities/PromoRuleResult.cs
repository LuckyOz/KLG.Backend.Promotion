
using KLG.Library.Microservice.DataAccess;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KLG.Backend.Promotion.Services.Entities
{
    [Table("promo_rule_result")]
    public class PromoRuleResult : KLGModelBase
    {
        [Column("promo_rule_id"), MaxLength(40)]
        [ForeignKey("PromoRule")]
        public string PromoruleId { get; set; }
        public PromoRule PromoRule { get; set; }

        [Column("linenum")]
        public int Linenum { get; set; }

        [Column("group_line")]
        public int Groupline { get; set; }

        [Column("item"), MaxLength(100)]
        public string Item { get; set; }

        [Column("dsc_value"), MaxLength(200)]
        public string DscValue { get; set; }

        [Column("max_value"), MaxLength(200)]
        public string MaxValue { get; set; }

        [Column("link_rsl"), MaxLength(10)]
        public string Linkrsl { get; set; }
    }
}
