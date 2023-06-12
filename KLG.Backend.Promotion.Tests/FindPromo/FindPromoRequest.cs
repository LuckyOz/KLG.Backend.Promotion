
using KLG.Backend.Promotion.Models.Request;

namespace KLG.Backend.Promotion.Tests.FindPromo
{
    public static class FindPromoRequest
    {
        public static PromoRequestDto Find_cart_amount_all => new()
        {
            TransDate = "25-12-2022",
            PromoCode = new()
            {

            },
            ItemProduct = new()
            {
                new ItemProduct()
                {
                    SkuCode = "milktest1",
                    SkuGroup = "milk",
                    Qty = 10,
                    Price = 10000
                },
                new ItemProduct()
                {
                    SkuCode = "milktest2",
                    SkuGroup = "milk",
                    Qty = 5,
                    Price = 10000
                }
            },
            Zone = "",
            Site = "",
            MemberCode = "",
            PromoType = "ed95cc28-d408-4d3c-9736-3024c7a4f8f4"
        };
    }
}
