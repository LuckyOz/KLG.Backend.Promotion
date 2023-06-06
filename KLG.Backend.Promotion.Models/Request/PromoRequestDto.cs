
using Newtonsoft.Json;

namespace KLG.Backend.Promotion.Models.Request
{
    public class PromoRequestDto
    {
        [JsonProperty("transDate")]
        public string TransDate { get; set; }
        [JsonProperty("promoCode")]
        public List<string> PromoCode { get; set; }
        [JsonProperty("itemProduct")]
        public List<ItemProduct> ItemProduct { get; set; }
        [JsonProperty("zone")]
        public string Zone { get; set; }
        [JsonProperty("site")]
        public string Site { get; set; }
        [JsonProperty("memberCode")]
        public string MemberCode { get; set; }
        [JsonProperty("promoType")]
        public string PromoType { get; set; }
    }

    public class ItemProduct
    {
        [JsonProperty("skuCode")]
        public string SkuCode { get; set; }
        [JsonProperty("skuGroup")]
        public string SkuGroup { get; set; }
        [JsonProperty("qty")]
        public int Qty { get; set; }
        [JsonProperty("price")]
        public decimal Price { get; set; }
    }
}
