
namespace KLG.Backend.Promotion.Models.Request
{
    public class ValidatePromoResponseDto
    {
        public string TransDate { get; set; }
        public string Zone { get; set; }
        public string Site { get; set; }
        public string MemberCode { get; set; }
        public string PromoType { get; set; }
        public List<ItemProductValidate> ItemProduct { get; set; }
    }
    public class ItemProductValidate
    {
        public string SkuCode { get; set; }
        public string SkuGroup { get; set; }
        public List<PromoProductValidate> PromoProduct { get; set; }
    }

    public class PromoProductValidate
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int Cls { get; set; }
        public int Lv { get; set; }
        public string Type { get; set; }
        public string TypeResult { get; set; }
        public string Value { get; set; }
        public string ValueMax { get; set; }
    }
}
