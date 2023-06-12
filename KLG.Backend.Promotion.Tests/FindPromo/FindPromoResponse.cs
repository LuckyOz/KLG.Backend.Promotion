
using KLG.Backend.Promotion.Models.Request;

namespace KLG.Backend.Promotion.Tests.FindPromo
{
    public static class FindPromoResponse
    {
        public static ServiceResponse<List<FindPromoResponseDto>> Find_cart_amount_all => new()
        {
            Data = new List<FindPromoResponseDto>()
            {
                new FindPromoResponseDto()
                {
                    Group = "ed95cc28-d408-4d3c-9736-3024c7a4f8f4",
                    Code = "2c4ce78f-f8e8-4dc5-ae0a-06c8423a966d",
                    Name = "Promo Happy New Year",
                    Type = "AMOUNT",
                    TypeResult = "ALL",
                    ValDiscount = "5000",
                    ValMaxDiscount = "",
                    Cls = 2,
                    Lvl = 2,
                    PromoListItem = new()
                    {
                        new PromoListItem()
                        {
                            TotalBefore = 150000,
                            TotalDiscount = 5000,
                            TotalAfter = 145000,
                            Rounding = 1,
                            PromoListItemDetail = new()
                            {
                                new PromoListItemDetail()
                                {
                                    SkuCode = "milktest1",
                                    ValDiscount = "5000",
                                    ValMaxDiscount = "",
                                    Price = 10000,
                                    Qty = 10,
                                    TotalPrice = 100000,
                                    TotalDiscount = 3333,
                                    TotalAfter = 96667
                                },
                                new PromoListItemDetail()
                                {
                                    SkuCode = "milktest2",
                                    ValDiscount = "5000",
                                    ValMaxDiscount = "",
                                    Price = 10000,
                                    Qty = 5,
                                    TotalPrice = 50000,
                                    TotalDiscount = 1666,
                                    TotalAfter = 48334
                                },
                            }
                        }
                    }
                }
            },
            Success = true,
            Message = "Success",
            Pages = 1,
            TotalPages = 1
        };
    }
}
