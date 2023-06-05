
namespace KLG.Backend.Promotion.Models.Request
{
    public class FindPromoResponseDto
    {
        public string Group { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string TypeResult { get; set; }
        public string ValDiscount { get; set; }
        public string ValMaxDiscount { get; set; }
        public int Cls { get; set; }
        public int Lvl { get; set; }
        public List<PromoListItem> PromoListItem { get; set; }

        public FindPromoResponseDto() { }

        public FindPromoResponseDto(FindPromoResponseDto other)
        {
            this.Group = other.Group;
            this.Code = other.Code;
            this.Name = other.Name;
            this.Type = other.Type;
            this.TypeResult = other.TypeResult;
            this.ValDiscount = other.ValDiscount;
            this.ValMaxDiscount = other.ValMaxDiscount;
            this.Cls = other.Cls;
            this.Lvl = other.Lvl;
            this.PromoListItem = other.PromoListItem;
        }
    }

    public class PromoListItem
    {
        public double TotalBefore { get; set; }
        public double TotalDiscount { get; set; }
        public double TotalAfter { get; set; }
        public int Rounding { get; set; }
        public List<PromoListItemDetail> PromoListItemDetail { get; set; }

        public PromoListItem() { }

        public PromoListItem(PromoListItem other)
        {
            this.TotalBefore = other.TotalBefore;
            this.TotalDiscount = other.TotalDiscount;
            this.TotalAfter = other.TotalAfter;
            this.Rounding = other.Rounding;
            this.PromoListItemDetail = other.PromoListItemDetail;
        }
    }

    public class PromoListItemDetail
    {
        public string SkuCode { get; set; }
        public string ValDiscount { get; set; }
        public string ValMaxDiscount { get; set; }
        public double Price { get; set; }
        public double Qty { get; set; }
        public double TotalPrice { get; set; }
        public double TotalDiscount { get; set; }
        public double TotalAfter { get; set; }

        public PromoListItemDetail() { }
    }

    public class ItemGroupResultPerPromo
    {
        public string Item { get; set; }
        public string Value { get; set; }
        public string MaxValue { get; set; }
    }
}
