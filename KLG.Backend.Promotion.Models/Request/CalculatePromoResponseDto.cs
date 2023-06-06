using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLG.Backend.Promotion.Models.Request
{
    public class CalculatePromoResponseDto
    {
        public decimal TotalBefore { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalAfter { get; set; }
        public int TotalRounding { get; set; }
        public List<Promo> Promo { get; set; }
        public List<ItemProductPromoDetail> ItemProducts { get; set; }
    }

    public class Promo
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string ValDiscount { get; set; }
        public string ValMaxDiscount { get; set; }
        public int Cls { get; set; }
        public int Lvl { get; set; }
        public int Rounding { get; set; }
        public List<ItemProductPromo> ItemPromo { get; set; }
    }

    public class ItemProductPromo
    {
        public string val_discount { get; set; }
        public string val_max_discount { get; set; }
        public string sku_code { get; set; }
        public decimal discount { get; set; }
    }

    public class ItemProductPromoDetail
    {
        public string SkuCode { get; set; }
        public string SkuGroup { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public decimal TotalBefore { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalAfter { get; set; }
    }

    //Kebutuhan Calculate Promo by Level
    public class ItemProductPromoDetailCalculate
    {
        public string SkuCode { get; set; }
        public string SkuGroup { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public decimal PriceTemp { get; set; }
        public decimal TotalBefore { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalAfter { get; set; }
    }
}
