
using Newtonsoft.Json;
using RulesEngine.Models;
using RulesEngine.Actions;
using KLG.Backend.Promotion.Models.Request;
using KLG.Backend.Promotion.Services.Entities;

namespace KLG.Backend.Promotion.Services.Configuration
{
    public class PromoResult : ActionBase
    {
        public override async ValueTask<object> Run(ActionContext context, RuleParameter[] ruleParameters)
        {
            //Get Data Cart
            PromoRequestDto dataCart = JsonConvert.DeserializeObject<PromoRequestDto>(
                                        JsonConvert.SerializeObject(
                                            ruleParameters.Where(q => q.Name == "paramsPromo").Select(q => q.Value).FirstOrDefault()
                                            )
                                        );

            //Get Data Promo Result
            var dataPromo = JsonConvert.DeserializeObject<PromoRule>(context.GetContext<string>("datapromo"));

            //Return Calculate per Promo
            return await CalculatePromoSingle(dataPromo, dataCart);
        }

        public async Task<FindPromoResponseDto> CalculatePromoSingle(PromoRule dataPromo, PromoRequestDto dataCart)
        {
            FindPromoResponseDto response = new(); //Varibale untuk save data result promo header
            List<PromoListItem> responseDetail = new(); //Variable untuk save data detail item semua group

            //Execute Promo Untuk Semua Item
            if (dataPromo.ItemType == "ALL") {
                
                List<PromoListItemDetail> responseDetailGroup = new();

                // Looping Item di Cart untuk Execute Promo
                foreach (var loopItemCart in dataCart.ItemProduct) {
                    
                    decimal discountTypeAll = 0;

                    if (dataPromo.PromoActionType == "AMOUNT") {
                        //Execute Promo Amount

                        if (dataPromo.Cls > 1) {
                            //Execute Promo Cart & Additional

                            //Get Total Harga Per SKU
                            decimal totalPriceSku = loopItemCart.Qty * loopItemCart.Price;

                            //Get Total Harga All Cart
                            decimal totalPriceCart = dataCart.ItemProduct.Sum(q => q.Price * q.Qty);

                            //Get Discount Prorate di Cart
                            discountTypeAll = Math.Floor(totalPriceSku / totalPriceCart * Convert.ToDecimal(dataPromo.PromoActionValue));
                        } else {
                            //Execute Promo Item
                            discountTypeAll = Convert.ToDecimal(dataPromo.PromoActionValue);
                        }
                    }

                    if (dataPromo.PromoActionType == "PERCENT") {
                        //Execute Promo Percent

                        discountTypeAll = loopItemCart.Qty * loopItemCart.Price * Convert.ToDecimal(dataPromo.PromoActionType.Replace("%", "")) / 100;
                    }

                    PromoListItemDetail ResponseDetailGroupSingle = new()
                    {
                        SkuCode = loopItemCart.SkuCode,
                        ValDiscount = dataPromo.PromoActionValue,
                        ValMaxDiscount = dataPromo.MaxValue,
                        Price = Convert.ToDouble(loopItemCart.Price),
                        Qty = loopItemCart.Qty,
                        TotalPrice = Convert.ToDouble(loopItemCart.Price * loopItemCart.Qty),
                        TotalDiscount = Convert.ToDouble(discountTypeAll),
                        TotalAfter = Convert.ToDouble(loopItemCart.Price * loopItemCart.Qty - discountTypeAll)
                    };

                    responseDetailGroup.Add(ResponseDetailGroupSingle);
                }

                //Declare for Result Detail Promo
                int roundingAllItem = dataPromo.PromoActionType == "AMOUNT" && dataPromo.Cls > 1 ?
                                Convert.ToInt32(dataPromo.PromoActionValue) - Convert.ToInt32(responseDetailGroup.Sum(q => q.TotalDiscount))
                                : 0;
                double total_beforeAllItem = Convert.ToDouble(dataCart.ItemProduct.Sum(q => q.Qty * q.Price));
                double total_discountAllItem = responseDetailGroup.Sum(q => q.TotalDiscount) + Convert.ToDouble(roundingAllItem);
                double total_afterAllItem = total_beforeAllItem - total_discountAllItem;

                PromoListItem responseDetailSingle = new()
                {
                    Rounding = roundingAllItem,
                    TotalBefore = total_beforeAllItem,
                    TotalDiscount = total_discountAllItem,
                    TotalAfter = total_afterAllItem,
                    PromoListItemDetail = responseDetailGroup
                };

                responseDetail.Add(responseDetailSingle);
            }
            //Execute Promo Untuk Custom Item
            else if (dataPromo.ItemType == "CUSTOM") {
                //Grouping item bedasarkan group di db ms_promo_rule_result
                List<List<ItemGroupResultPerPromo>> listItemPerPromo = new(); //Variable untuk menampung item custom group
                var groupList = dataPromo.PromoRuleResult.Select(q => q.Groupline).Distinct().ToList(); //get data disctinc group

                //Looping group untuk mendapatkan item
                foreach (var loopGroup in groupList) {
                    List<ItemGroupResultPerPromo> listItemPerGroup = new(); //Varibel untuk menampung item di 1 group
                    var listItemGroup = dataPromo.PromoRuleResult.Where(q => q.Groupline == loopGroup).ToList(); //get data item sesuai looping group

                    foreach (var loopItemGroup in listItemGroup) {
                        //Input data item ke list dalam 1 group
                        ItemGroupResultPerPromo ItemPerPromo = new()
                        {
                            Item = loopItemGroup.Item,
                            Value = loopItemGroup.DscValue,
                            MaxValue = loopItemGroup.MaxValue
                        };

                        listItemPerGroup.Add(ItemPerPromo);
                    }

                    listItemPerPromo.Add(listItemPerGroup);
                }

                //Looping item per group
                foreach (var loopItemperPromo in listItemPerPromo) {

                    List<PromoListItemDetail> responseDetailGroup = new(); //Model for save hasil promo item per group

                    decimal discountTypeCustom = 0;
                    List<string> dataListItem = new();

                    foreach (var loopGetDataItemPerPromo in loopItemperPromo) {
                        dataListItem.Add(loopGetDataItemPerPromo.Item);
                    }

                    /* Start Untuk Cek Item AND Jika tidak ada salah satu maka tidak dapat promo */
                    List<string> dataItemGroup = new();

                    foreach (var loopItemGroupString in loopItemperPromo) {
                        dataItemGroup.Add(loopItemGroupString.Item);
                    }

                    bool executePromoCekResultType = false;
                    var cekItemGroupInCart = dataCart.ItemProduct.Where(q => dataItemGroup.Contains(q.SkuCode));

                    executePromoCekResultType = cekItemGroupInCart.Count() == loopItemperPromo.Count || dataPromo.ResultType == "V2";

                    if (executePromoCekResultType) {
                        foreach (var loopItemPerGroup in loopItemperPromo) {
                            PromoListItemDetail responseDetailGroupSingle = new();
                            var dataItemCart = dataCart.ItemProduct.FirstOrDefault(q => q.SkuCode == loopItemPerGroup.Item); //Get data di cart

                            if (dataItemCart != null) {


                                if (dataPromo.PromoActionType == "AMOUNT") { //Execute Promo Amount
                                    if (dataPromo.Cls > 1) {
                                        //Execute Untuk Class Selain Item
                                        //Get Total Harga Per SKU
                                        decimal totalPriceSku = dataItemCart.Qty * dataItemCart.Price;

                                        //Get Total Harga Custom Item
                                        decimal totalPriceCart = dataCart.ItemProduct.Where(q => dataListItem.Contains(q.SkuCode)).Sum(q => q.Price * q.Qty);

                                        //Get Discount Prorate di Cart Untuk Custom Item
                                        discountTypeCustom = Math.Floor(totalPriceSku / totalPriceCart * Convert.ToDecimal(dataPromo.PromoActionValue));
                                    } else {
                                        //Execute Untuk Class Item
                                        discountTypeCustom = Convert.ToDecimal(loopItemPerGroup.Value);
                                    }
                                } else if (dataPromo.PromoActionType == "PERCENT") { //Execute Promo Percent
                                    discountTypeCustom = dataItemCart.Price * dataItemCart.Qty * Convert.ToDecimal(loopItemPerGroup.Value.Replace("%", "")) / 100;
                                } else if (dataPromo.PromoActionType == "ITEM") { //Execute Promo Item
                                    discountTypeCustom = dataItemCart.Price;
                                } else if (dataPromo.PromoActionType == "SP") { //Execute Promo Special Price
                                    discountTypeCustom = (dataItemCart.Price - Convert.ToDecimal(loopItemPerGroup.Value)) * Convert.ToDecimal(dataItemCart.Qty);
                                } else {
                                    discountTypeCustom = 0;
                                }

                                responseDetailGroupSingle.SkuCode = dataItemCart.SkuCode;
                                responseDetailGroupSingle.ValDiscount = loopItemPerGroup.Value;
                                responseDetailGroupSingle.ValMaxDiscount = loopItemPerGroup.MaxValue;
                                responseDetailGroupSingle.Price = Convert.ToDouble(dataItemCart.Price);
                                responseDetailGroupSingle.Qty = dataItemCart.Qty;
                                responseDetailGroupSingle.TotalPrice = Convert.ToDouble(dataItemCart.Price * dataItemCart.Qty);
                                responseDetailGroupSingle.TotalDiscount = Convert.ToDouble(discountTypeCustom);
                                responseDetailGroupSingle.TotalAfter = Convert.ToDouble(dataItemCart.Price * dataItemCart.Qty - discountTypeCustom);

                                responseDetailGroup.Add(responseDetailGroupSingle);
                            }
                        }
                    }

                    if (responseDetailGroup.Count > 0) {
                        //Declare for Result Detail Promo
                        int roundingAllItem = dataPromo.PromoActionType == "AMOUNT" && dataPromo.Cls > 1 ?
                                        Convert.ToInt32(dataPromo.PromoActionValue) - Convert.ToInt32(responseDetailGroup.Sum(q => q.TotalDiscount))
                                        : 0;
                        double total_beforeAllItem = Convert.ToDouble(dataCart.ItemProduct.Sum(q => q.Qty * q.Price));
                        double total_discountAllItem = responseDetailGroup.Sum(q => q.TotalDiscount) + Convert.ToDouble(roundingAllItem);
                        double total_afterAllItem = total_beforeAllItem - total_discountAllItem;

                        PromoListItem responseDetailSingle = new()
                        {
                            Rounding = roundingAllItem,
                            TotalBefore = total_beforeAllItem,
                            TotalDiscount = total_discountAllItem,
                            TotalAfter = total_afterAllItem,
                            PromoListItemDetail = responseDetailGroup
                        };

                        responseDetail.Add(responseDetailSingle);
                    }
                }
            } 
            
            if (responseDetail.Count > 0) {
                //Save data to Response
                response.Group = dataPromo.PromoworkflowId;
                response.Code = dataPromo.Id;
                response.Name = dataPromo.Name;
                response.Type = dataPromo.PromoActionType;
                response.TypeResult = dataPromo.ItemType;
                response.ValDiscount = dataPromo.PromoActionValue;
                response.ValMaxDiscount = dataPromo.MaxValue;
                response.Cls = dataPromo.Cls;
                response.Lvl = dataPromo.Lvl;
                response.PromoListItem = responseDetail;
            }

            return response;
        }
    }
}
