﻿
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using KLG.Library.Microservice.DataAccess;
using KLG.Backend.Promotion.Models.Request;
using KLG.Backend.Promotion.Services.Entities;
using KLG.Backend.Promotion.Services.Resources;
using KLG.Backend.Promotion.Services.Configuration;

namespace KLG.Backend.Promotion.Services.Business.Promotion
{
    public class PromoManager : IPromoManager
    {
        private readonly IPromoSetup _promoSetup;
        private readonly IKLGDbProvider<DefaultDbContext> _dbProvider;

        public PromoManager(IPromoSetup promoSetup,
            IKLGDbProvider<DefaultDbContext> dbProvider)
        {
            _promoSetup = promoSetup;
            _dbProvider = dbProvider;
        }

        public async Task<bool> InsertDataDefault()
        {
            DbTesting myDb = new();

            _dbProvider.DbContext.PromoWorkflow.AddRange(myDb.listmspromoworkflow);
            await _dbProvider.DbContext.SaveChangesAsync();

            return true;
        }

        public async Task<ServiceResponse<List<FindPromoResponseDto>>> FindPromo(PromoRequestDto promoRequestDto)
        {
            List<FindPromoResponseDto> listPromoResult = new();
            ServiceResponse<List<FindPromoResponseDto>> response = new();

            try
            {
                var promoResult = await _promoSetup.GetPromo(promoRequestDto.PromoType, promoRequestDto);
                promoResult = promoResult.Where(q => q.IsSuccess).ToList();

                foreach (var loopPromoResult in promoResult)
                {
                    var dataResultDetailString = JsonConvert.SerializeObject(loopPromoResult.ActionResult.Output);
                    var dataResultDetail = JsonConvert.DeserializeObject<FindPromoResponseDto>(dataResultDetailString);

                    if (dataResultDetail.Code != null)
                    {
                        listPromoResult.Add(dataResultDetail);
                    }
                }

                if (listPromoResult.Count < 1)
                {
                    response.Success = false;
                    response.Message = "No Have Promo for This Cart";

                    return response;
                }

                response.Data = listPromoResult;

                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;

                return response;
            }

        }

        public async Task<ServiceResponse<ValidatePromoResponseDto>> ValidatePromo(PromoRequestDto promoRequestDto)
        {
            List<ItemProductValidate> listDataItemValidate = new();
            ServiceResponse<ValidatePromoResponseDto> response = new();

            try
            {
                var responseFind = await this.FindPromo(promoRequestDto);

                if (responseFind.Success && responseFind.Data.Count > 0)
                {

                    //Variable for save promo
                    List<FindPromoResponseDto> promoListDataSave = new();

                    //get list class for looping
                    var getListClass = responseFind.Data.Select(q => q.Cls).Distinct().ToList();

                    //looping class
                    foreach (var loopListClass in getListClass)
                    {

                        //get data promo bedasarkan class
                        var cekPromoAllItem = responseFind.Data.Where(q => q.Cls == loopListClass && q.TypeResult == "ALL").ToList(); //get data promo untuk apply all item
                        var cekPromoCustomItem = responseFind.Data.Where(q => q.Cls == loopListClass && q.TypeResult == "CUSTOM").ToList(); //get data promo untuk apply custom item

                        if (cekPromoAllItem.Count > 0)
                        {

                            FindPromoResponseDto promoDataSaveAll;

                            if (cekPromoAllItem.Count == 1)
                            { //cek jika hanya terdapat 1 promo untuk apply all item
                                promoDataSaveAll = cekPromoAllItem.FirstOrDefault();
                            }
                            else
                            {
                                //jika promo untuk apply all item lebih dari 1
                                if (cekPromoAllItem.Any(q => q.Lvl == 1))
                                {
                                    //menangin promo apply all item untuk result get item
                                    //order promo untuk dapat promo yang terbesar
                                    promoDataSaveAll = cekPromoAllItem.Where(q => q.Lvl == 1).OrderByDescending(q => q.PromoListItem
                                                                   .Sum(q => q.TotalDiscount)).FirstOrDefault();
                                }
                                else
                                {
                                    //promo apply all item result selain get item
                                    //order promo untuk dapat promo yang terbesar
                                    promoDataSaveAll = cekPromoAllItem.OrderByDescending(q => q.PromoListItem
                                                                   .Sum(q => q.TotalDiscount)).FirstOrDefault();
                                }
                            }

                            promoListDataSave.Add(promoDataSaveAll);

                        }
                        else
                        {//cek jika terdapat promo apply custom item dan tidak ada apply all item

                            if (cekPromoCustomItem.Count == 1)
                            {
                                //jika hanya terdapat 1 promo untuk apply custom item

                                FindPromoResponseDto promoDataSaveCustom;

                                if (cekPromoCustomItem[0].PromoListItem.Count == 1)
                                {
                                    //jika hanya terdapat 1 kombinasi di 1 promo untuk apply custom item
                                    promoDataSaveCustom = cekPromoCustomItem[0];

                                }
                                else
                                {

                                    //jika terdapat lebih dari 1 kombinasi untuk apply custom item
                                    List<PromoListItem> newDataList = new()
                                    {
                                        //ambil promo terbesar untuk apply custom item
                                        new PromoListItem(cekPromoCustomItem[0].PromoListItem.OrderByDescending(q => q.TotalDiscount).FirstOrDefault())
                                    };

                                    FindPromoResponseDto newData = cekPromoCustomItem[0];
                                    newData.PromoListItem = newDataList; //replace list dengan data promo yang terbesar

                                    promoDataSaveCustom = newData;
                                }

                                promoListDataSave.Add(promoDataSaveCustom);
                            }
                            else
                            {
                                //jika terdapat lebih dari 1 promo untuk apply custom item

                                //Looping untuk mendapatkan kombinasi jumlah item

                                List<List<int>> listlistItemPromoCombination = new();

                                foreach (var loopPromoCustomItem in cekPromoCustomItem)
                                {
                                    List<int> listItemPromoCombination = new();

                                    for (int i = 0; i < loopPromoCustomItem.PromoListItem.Count; i++)
                                    {
                                        listItemPromoCombination.Add(i);
                                    }

                                    listlistItemPromoCombination.Add(listItemPromoCombination);
                                }

                                var resultCombi = CombineList(listlistItemPromoCombination);

                                //looping untuk implementasi jumlah item ke item dengan discountnya
                                List<List<FindPromoResponseDto>> listListNewDataPromoTemp = new();

                                foreach (var loopResultCombi in resultCombi)
                                {
                                    int count = 0;

                                    List<FindPromoResponseDto> listNewDataPromoTemp = new();
                                    List<FindPromoResponseDto> listNewDataPromoTempDelete = new();

                                    foreach (var loopLoopResultCombi in loopResultCombi)
                                    {

                                        FindPromoResponseDto newDataPromoTemp = new(cekPromoCustomItem[count])
                                        {
                                            PromoListItem = new()
                                            {
                                                new PromoListItem(cekPromoCustomItem[count].PromoListItem[loopLoopResultCombi])
                                            }
                                        };

                                        bool cekInsert = true;

                                        foreach (var loopCekItemSameGroup in newDataPromoTemp.PromoListItem)
                                        {
                                            foreach (var loopCekItemSameHead in loopCekItemSameGroup.PromoListItemDetail)
                                            {
                                                foreach (var loopItemSameGroupHasAdd in listNewDataPromoTemp)
                                                {
                                                    foreach (var loopCekItemSameHeadHasAdd in loopItemSameGroupHasAdd.PromoListItem)
                                                    {
                                                        foreach (var loopItemSameDetailHasAdd in loopCekItemSameHeadHasAdd.PromoListItemDetail)
                                                        {
                                                            if (loopCekItemSameHead.SkuCode == loopItemSameDetailHasAdd.SkuCode)
                                                            {

                                                                decimal itemDisc01 = 0, itemDisc02 = 0;

                                                                if (loopItemSameDetailHasAdd.ValDiscount == "FREE")
                                                                {
                                                                    itemDisc01 = Convert.ToDecimal(loopItemSameDetailHasAdd.Price);
                                                                }
                                                                else
                                                                {
                                                                    itemDisc01 = loopItemSameDetailHasAdd.ValDiscount.Contains('%') ?
                                                                        Convert.ToDecimal(loopItemSameDetailHasAdd.ValDiscount.Replace("%", "")) :
                                                                        Convert.ToDecimal(loopItemSameDetailHasAdd.ValDiscount);
                                                                }

                                                                if (loopCekItemSameHead.ValDiscount == "FREE")
                                                                {
                                                                    itemDisc02 = Convert.ToDecimal(loopCekItemSameHead.Price);
                                                                }
                                                                else
                                                                {
                                                                    itemDisc02 = loopCekItemSameHead.ValDiscount.Contains('%') ?
                                                                        Convert.ToDecimal(loopCekItemSameHead.ValDiscount.Replace("%", "")) :
                                                                        Convert.ToDecimal(loopCekItemSameHead.ValDiscount);
                                                                }

                                                                if (itemDisc01 >= itemDisc02)
                                                                {
                                                                    cekInsert = false;
                                                                }
                                                                else
                                                                {
                                                                    listNewDataPromoTempDelete.Add(loopItemSameGroupHasAdd);
                                                                }

                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        if (listNewDataPromoTempDelete.Count > 0)
                                        {
                                            foreach (var loopDeleteList in listNewDataPromoTempDelete)
                                            {
                                                listNewDataPromoTemp.Remove(loopDeleteList);
                                            }
                                        }

                                        if (cekInsert)
                                        {
                                            listNewDataPromoTemp.Add(newDataPromoTemp);
                                        }

                                        count++;
                                    }

                                    listListNewDataPromoTemp.Add(listNewDataPromoTemp);
                                }

                                //Get Promo Tertinggi
                                int countSum = 0;
                                Dictionary<int, double> cekListSum = new();

                                foreach (var loopGetPromoOrder in listListNewDataPromoTemp)
                                {
                                    double sumPromo = 0;

                                    foreach (var loopSumCek in loopGetPromoOrder)
                                    {
                                        sumPromo += Convert.ToDouble(loopSumCek.PromoListItem[0].TotalDiscount);
                                    }

                                    cekListSum.Add(countSum, sumPromo);

                                    countSum++;
                                }

                                var dataFinalValidateNumber = cekListSum.OrderByDescending(q => q.Value).FirstOrDefault();
                                var dataFinalValidatePromo = listListNewDataPromoTemp[dataFinalValidateNumber.Key];

                                promoListDataSave.AddRange(dataFinalValidatePromo);
                            }
                        }
                    }

                    foreach (var loopItemRequest in promoRequestDto.ItemProduct)
                    {

                        List<PromoProductValidate> listDataPromoforItem = new();

                        foreach (var loopDataProtmoList in promoListDataSave)
                        {

                            if (loopDataProtmoList.PromoListItem.Count < 1)
                            {
                                continue;
                            }

                            if (loopDataProtmoList.PromoListItem[0].PromoListItemDetail.Count < 1)
                            {
                                continue;
                            }

                            var dataPromoPerItem = loopDataProtmoList.PromoListItem[0]
                                                    .PromoListItemDetail
                                                    .FirstOrDefault(q => q.SkuCode == loopItemRequest.SkuCode);

                            if (dataPromoPerItem != null)
                            {

                                PromoProductValidate DataPromoforItem = new()
                                {
                                    Code = loopDataProtmoList.Code,
                                    Name = loopDataProtmoList.Name,
                                    Cls = loopDataProtmoList.Cls,
                                    Lv = loopDataProtmoList.Lvl,
                                    Type = loopDataProtmoList.Type,
                                    TypeResult = loopDataProtmoList.TypeResult,
                                    Value = loopDataProtmoList.ValDiscount != "" ? loopDataProtmoList.ValDiscount : dataPromoPerItem.ValDiscount,
                                    ValueMax = loopDataProtmoList.ValMaxDiscount
                                };

                                listDataPromoforItem.Add(DataPromoforItem);
                            }
                        }

                        ItemProductValidate DataItemValidate = new()
                        {
                            SkuCode = loopItemRequest.SkuCode,
                            SkuGroup = loopItemRequest.SkuGroup,
                            PromoProduct = listDataPromoforItem
                        };

                        listDataItemValidate.Add(DataItemValidate);
                    }

                }
                else
                {

                    foreach (var loopItemRequest in promoRequestDto.ItemProduct)
                    {

                        ItemProductValidate DataItemValidate = new()
                        {
                            SkuCode = loopItemRequest.SkuCode,
                            SkuGroup = loopItemRequest.SkuGroup
                        };

                        listDataItemValidate.Add(DataItemValidate);
                    }
                }

                ValidatePromoResponseDto dataValidatePromo = new()
                {
                    TransDate = promoRequestDto.TransDate,
                    Zone = promoRequestDto.Zone,
                    Site = promoRequestDto.Site,
                    MemberCode = promoRequestDto.MemberCode,
                    PromoType = promoRequestDto.PromoType,
                    ItemProduct = listDataItemValidate
                };

                response.Data = dataValidatePromo;

                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;

                return response;
            }
        }

        public async Task<ServiceResponse<CalculatePromoResponseDto>> CalculatePromo(PromoRequestDto promoRequestDto, List<string> listmaxDiscount = null)
        {

            ServiceResponse<CalculatePromoResponseDto> response = new();

            try
            {
                int cekClassforPrice = 0;
                List<Promo> listPromo = new();
                List<ItemProductPromoDetail> listItemProductPromoDetail = new();
                List<ItemProductPromoDetailCalculate> listItemProductPromoDetailCalculate = new();

                //get data promo in redis
                List<PromoRule> listDataPromo = await _dbProvider.DbContext.PromoRule
                    .Where(q => q.PromoworkflowId == promoRequestDto.PromoType && promoRequestDto.PromoCode.Contains(q.Id))
                    .Include(q => q.PromoRuleResult)
                    .ToListAsync();

                var listClass = listDataPromo.OrderBy(q => q.Cls).Select(q => q.Cls).Distinct().ToList();

                foreach (var loopListClass in listClass)
                {

                    //Untuk order get item terlebih dahulu
                    var listDataPromoClass = listDataPromo.Where(q => q.Cls == loopListClass).OrderBy(q => q.Lvl).ToList();

                    //Looping discount apply
                    foreach (var loopDataPromo in listDataPromoClass)
                    {

                        bool cekHaveMaxDiscount = false;

                        if (listmaxDiscount.Any(q => q == loopDataPromo.Id))
                        {
                            loopDataPromo.PromoActionType = "AMOUNT";
                            loopDataPromo.PromoActionValue = loopDataPromo.MaxValue;
                            cekHaveMaxDiscount = true;
                        }

                        //Model Save Cek Total Discount
                        List<ItemProductPromo> listItemProductPromo = new();

                        //Save Item Discount to List
                        List<string> listItemResult = new();

                        //Variable for Save rounding
                        decimal totalDiscountAmount = 0;

                        //Save Variable List for Cek Item in Custom
                        if (loopDataPromo.ItemType == "CUSTOM")
                        {
                            foreach (var loopItemResult in loopDataPromo.PromoRuleResult)
                            {
                                listItemResult.Add(loopItemResult.Item);
                            }
                        }

                        //Execute Discount
                        if (listItemProductPromoDetailCalculate.Count > 0)
                        {
                            //Looping untuk execute discount yang sudah pernah dilakukan sebelumnya menggunakan model discount
                            foreach (var loopItemModelItemDiscount in listItemProductPromoDetailCalculate)
                            {
                                //Variable for Save Discount
                                decimal discount = 0;

                                //Setup Price , Cek if anotherclass using price temp
                                decimal usingPrice = cekClassforPrice > 0 ? loopItemModelItemDiscount.PriceTemp : loopItemModelItemDiscount.Price;

                                //Execute Discount Percent
                                if (loopDataPromo.PromoActionType == "PERCENT")
                                {

                                    if (loopDataPromo.ItemType == "ALL")
                                    {

                                        //Execute Discount Percent to All Item.
                                        discount = (usingPrice * loopItemModelItemDiscount.Qty)
                                                    * Convert.ToDecimal(loopDataPromo.PromoActionValue.Replace("%", "")) / 100;

                                    }

                                    //Cek Item Avaible to Calculate Discount
                                    if (loopDataPromo.ItemType == "CUSTOM" && listItemResult.Any(q => q.Contains(loopItemModelItemDiscount.SkuCode)))
                                    {
                                        //Get Discount Percent Value in Rslt List
                                        string discValueRslt = loopDataPromo.PromoRuleResult
                                                            .Where(q => q.Item == loopItemModelItemDiscount.SkuCode)
                                                            .Select(q => q.DscValue)
                                                            .FirstOrDefault();

                                        //Execute Discount Percent to Custom Item
                                        discount = (usingPrice * loopItemModelItemDiscount.Qty) *
                                                    Convert.ToDecimal(discValueRslt.Replace("%", "")) / 100;
                                    }
                                }

                                //Execute Discount Amount 
                                if (loopDataPromo.PromoActionType == "AMOUNT")
                                {

                                    //Cek Promo di Class Item Atau Cart
                                    if (loopDataPromo.Cls == 1 && !cekHaveMaxDiscount)
                                    {
                                        //Promo Amount Class 1 / Level Item di apply hanya di Item tidak di prorate ke semua item cart

                                        //Cek Promo Type Item ke All Item atau Custom
                                        if (loopDataPromo.ItemType == "ALL")
                                        {

                                            //Cek Jika Max Value (Kelipatan Discount) Lebih dari Qty atau tidak atau 0
                                            if (loopItemModelItemDiscount.Qty > Convert.ToInt32(loopDataPromo.MaxValue)
                                                    || Convert.ToInt32(loopDataPromo.MaxValue) == 0 || loopDataPromo.MaxValue is null)
                                            {

                                                //Execute Discount Amount
                                                discount = Convert.ToDecimal(loopDataPromo.PromoActionValue) * loopItemModelItemDiscount.Qty;

                                            }
                                            else if (Convert.ToInt32(loopDataPromo.MaxValue) > loopItemModelItemDiscount.Qty)
                                            {

                                                //Execute Discount Amount
                                                discount = Convert.ToDecimal(loopDataPromo.PromoActionValue) * Convert.ToDecimal(loopDataPromo.MaxValue);
                                            }

                                        }

                                        //Cek Promo Amount Jika Custom Item
                                        //Cek Item Cart Termasuk ke Dalam Item Promo Atau Tidak
                                        if (loopDataPromo.ItemType == "CUSTOM" && listItemResult.Any(q => q.Contains(loopItemModelItemDiscount.SkuCode)))
                                        {
                                            //Get Discount Amount Value in Rslt List
                                            string discValueRslt = loopDataPromo.PromoRuleResult
                                                                .Where(q => q.Item == loopItemModelItemDiscount.SkuCode)
                                                                .Select(q => q.DscValue)
                                                                .FirstOrDefault();

                                            //Get Discount Max Value 
                                            string discMaxValue = loopDataPromo.PromoRuleResult
                                                                .Where(q => q.Item == loopItemModelItemDiscount.SkuCode)
                                                                .Select(q => q.MaxValue)
                                                                .FirstOrDefault();

                                            //Cek Value max value termasuk lebih dari qty / infinite base on qty / null
                                            if (Convert.ToInt32(discMaxValue) > loopItemModelItemDiscount.Qty
                                                || Convert.ToInt32(discMaxValue) == 0 || discMaxValue is null)
                                            {

                                                discount = Convert.ToDecimal(discValueRslt) * loopItemModelItemDiscount.Qty;

                                            }
                                            else if (Convert.ToInt32(discMaxValue) < loopItemModelItemDiscount.Qty)
                                            { //jika max value lebih dari qty
                                              // Execute Discount Amount
                                                discount = Convert.ToDecimal(discValueRslt) * Convert.ToDecimal(discMaxValue);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //Exec Promo Amount ada di class 2 atau di level cart akan di prorate ke semua item yang ada di cart

                                        //Untuk Calculate Total Semua Item Promo
                                        decimal totalPriceItemCart = 0;

                                        //Untuk Get Total Item Promo 
                                        decimal totalPriceSku = promoRequestDto.ItemProduct.Where(q => q.SkuCode == loopItemModelItemDiscount.SkuCode)
                                                                                            .Sum(q => q.Qty * q.Price);

                                        if (loopDataPromo.ItemType == "ALL")
                                        {
                                            //All Total Item at Cart
                                            totalPriceItemCart = promoRequestDto.ItemProduct.Sum(q => q.Price * q.Qty);
                                            discount = (totalPriceSku / totalPriceItemCart) * Convert.ToDecimal(loopDataPromo.PromoActionValue);

                                        }

                                        //Custom Total Item at Cart
                                        if (loopDataPromo.ItemType == "CUSTOM" && listItemResult.Any(q => q.Contains(loopItemModelItemDiscount.SkuCode)))
                                        {
                                            totalPriceItemCart = promoRequestDto.ItemProduct.Where(q => listItemResult
                                                                .Contains(q.SkuCode)).Sum(q => q.Price * q.Qty);

                                            discount = (totalPriceSku / totalPriceItemCart) * Convert.ToDecimal(loopDataPromo.PromoActionValue);
                                        }

                                        totalDiscountAmount += Convert.ToInt32(Math.Floor(discount));
                                    }
                                }

                                //Execute Discount Item
                                if (loopDataPromo.PromoActionType == "ITEM" && listItemResult.Any(q => q.Contains(loopItemModelItemDiscount.SkuCode)))
                                {

                                    int maxValue = Convert.ToInt32(loopDataPromo.PromoRuleResult
                                                            .Where(q => q.Item == loopItemModelItemDiscount.SkuCode)
                                                            .Select(q => q.MaxValue).FirstOrDefault());

                                    maxValue = maxValue > loopItemModelItemDiscount.Qty ? loopItemModelItemDiscount.Qty : maxValue;

                                    discount = loopItemModelItemDiscount.Price * maxValue;

                                }

                                loopItemModelItemDiscount.TotalDiscount += Convert.ToInt32(Math.Floor(discount));
                                loopItemModelItemDiscount.TotalAfter = loopItemModelItemDiscount.TotalBefore - loopItemModelItemDiscount.TotalDiscount;


                                //Save Data Promo to Model
                                if (Convert.ToInt32(Math.Floor(discount)) > 0)
                                {
                                    ItemProductPromo itemProductPromo = new()
                                    {
                                        sku_code = loopItemModelItemDiscount.SkuCode,
                                        discount = Convert.ToInt32(Math.Floor(discount)),

                                    };

                                    listItemProductPromo.Add(itemProductPromo);
                                }
                            }

                        }
                        else
                        {
                            //Looping untuk execute discount pertama kali dan menggunkan item dari cart dan hasilnya di masukan ke model discount
                            foreach (var loopItemCart in promoRequestDto.ItemProduct)
                            {

                                //Variable for Save Discount
                                decimal discount = 0;



                                //Execute Discount Percent
                                if (loopDataPromo.PromoActionType == "PERCENT")
                                {

                                    if (loopDataPromo.ItemType == "ALL")
                                    {

                                        //Execute Discount Percent to All Item.
                                        discount = (loopItemCart.Price * loopItemCart.Qty)
                                                    * Convert.ToDecimal(loopDataPromo.PromoActionValue.Replace("%", "")) / 100;

                                    }
                                    else if (loopDataPromo.ItemType == "CUSTOM" && listItemResult.Any(q => q.Contains(loopItemCart.SkuCode)))
                                    {

                                        //Get Discount Percent Value in Rslt List
                                        string discValueRslt = loopDataPromo.PromoRuleResult
                                                            .Where(q => q.Item == loopItemCart.SkuCode)
                                                            .Select(q => q.DscValue)
                                                            .FirstOrDefault();

                                        //Execute Discount Percent to Custom Item
                                        discount = (loopItemCart.Price * loopItemCart.Qty) *
                                                    Convert.ToDecimal(discValueRslt.Replace("%", "")) / 100;
                                    }

                                }

                                //Execute Discount Amount
                                if (loopDataPromo.PromoActionType == "AMOUNT")
                                {

                                    //Cek Promo di Class Item Atau Cart
                                    if (loopDataPromo.Cls == 1 && !cekHaveMaxDiscount)
                                    {

                                        //Promo Amount Class 1 / Level Item di apply hanya di Item tidak di prorate ke semua item cart

                                        //Cek Promo Type Item ke All Item atau Custom
                                        if (loopDataPromo.ItemType == "ALL")
                                        {

                                            //Cek Jika Max Value (Kelipatan Discount) Lebih dari Qty atau tidak atau 0
                                            if (loopItemCart.Qty > Convert.ToInt32(loopDataPromo.MaxValue)
                                                    || Convert.ToInt32(loopDataPromo.MaxValue) == 0 || loopDataPromo.MaxValue is null)
                                            {

                                                //Execute Discount Amount
                                                discount = Convert.ToDecimal(loopDataPromo.PromoActionValue) * loopItemCart.Qty;

                                            }
                                            else if (Convert.ToInt32(loopDataPromo.MaxValue) > loopItemCart.Qty)
                                            {

                                                //Execute Discount Amount
                                                discount = Convert.ToDecimal(loopDataPromo.PromoActionValue) * Convert.ToDecimal(loopDataPromo.MaxValue);
                                            }

                                        }
                                        else if (loopDataPromo.ItemType == "CUSTOM" && listItemResult.Any(q => q.Contains(loopItemCart.SkuCode)))
                                        { //Cek Promo Amount Jika Custom Item

                                            //Cek Item Cart Termasuk ke Dalam Item Promo Atau Tidak
                                            //Get Discount Amount Value in Rslt List
                                            string discValueRslt = loopDataPromo.PromoRuleResult
                                                                .Where(q => q.Item == loopItemCart.SkuCode)
                                                                .Select(q => q.DscValue)
                                                                .FirstOrDefault();

                                            string discMacValue = loopDataPromo.PromoRuleResult
                                                                .Where(q => q.Item == loopItemCart.SkuCode)
                                                                .Select(q => q.MaxValue)
                                                                .FirstOrDefault();

                                            //Cek Value max value termasuk lebih dari qty / infinite base on qty / null
                                            if (Convert.ToInt32(discMacValue) > loopItemCart.Qty
                                                || Convert.ToInt32(discMacValue) == 0 || discMacValue is null)
                                            {

                                                discount = Convert.ToDecimal(discValueRslt) * loopItemCart.Qty;

                                            }
                                            else if (Convert.ToInt32(discMacValue) < loopItemCart.Qty)
                                            { //jika max value lebih dari qty
                                              // Execute Discount Amount
                                                discount = Convert.ToDecimal(discValueRslt) * Convert.ToDecimal(discMacValue);
                                            }

                                        }
                                    }
                                    else
                                    {

                                        //Exec Promo Amount ada di class 2 atau di level cart akan di prorate ke semua item yang ada di cart

                                        //Untuk Calculate Total Semua Item Promo
                                        decimal totalPriceItemCart = 0;

                                        //Untuk Get Total Item Promo 
                                        decimal totalPriceSku = promoRequestDto.ItemProduct.Where(q => q.SkuCode == loopItemCart.SkuCode)
                                            .Sum(q => q.Qty * q.Price);

                                        if (loopDataPromo.ItemType == "ALL")
                                        {

                                            //All Total Item at Cart
                                            totalPriceItemCart = promoRequestDto.ItemProduct.Sum(q => q.Price * q.Qty);
                                            discount = (totalPriceSku / totalPriceItemCart) * Convert.ToDecimal(loopDataPromo.PromoActionValue);

                                        }
                                        else if (loopDataPromo.ItemType == "CUSTOM" && listItemResult.Any(q => q.Contains(loopItemCart.SkuCode)))
                                        {
                                            //Custom Total Item at Cart
                                            totalPriceItemCart = promoRequestDto.ItemProduct.Where(q => listItemResult
                                                                .Contains(q.SkuCode)).Sum(q => q.Price * q.Qty);

                                            discount = (totalPriceSku / totalPriceItemCart) * Convert.ToDecimal(loopDataPromo.PromoActionValue);
                                        }

                                        totalDiscountAmount += Convert.ToInt32(Math.Floor(discount));
                                    }

                                }

                                //Execute Discount Item
                                if (loopDataPromo.PromoActionType == "ITEM" && listItemResult.Any(q => q.Contains(loopItemCart.SkuCode)))
                                {
                                    int maxValue = Convert.ToInt32(loopDataPromo.PromoRuleResult
                                                            .Where(q => q.Item == loopItemCart.SkuCode)
                                                            .Select(q => q.MaxValue).FirstOrDefault());

                                    maxValue = maxValue > loopItemCart.Qty ? loopItemCart.Qty : maxValue;

                                    discount = loopItemCart.Price * maxValue;
                                }

                                //Save Data Item Execute Promo to Model Promo Response
                                ItemProductPromoDetailCalculate ItemProductPromoDetailCalculate = new()
                                {
                                    SkuCode = loopItemCart.SkuCode,
                                    SkuGroup = loopItemCart.SkuGroup,
                                    Qty = loopItemCart.Qty,
                                    Price = loopItemCart.Price,
                                    TotalBefore = loopItemCart.Qty * loopItemCart.Price,
                                    TotalDiscount = Convert.ToInt32(Math.Floor(discount)),
                                    TotalAfter = (loopItemCart.Qty * loopItemCart.Price) - Convert.ToInt32(Math.Floor(discount))
                                };

                                if (Convert.ToInt32(Math.Floor(discount)) > 0)
                                {

                                    ItemProductPromo itemProductPromo = new()
                                    {
                                        sku_code = loopItemCart.SkuCode,
                                        discount = Convert.ToInt32(Math.Floor(discount)),
                                        val_discount = loopDataPromo.ItemType == "CUSTOM" ?
                                            loopDataPromo.PromoRuleResult.FirstOrDefault(q => q.Item == loopItemCart.SkuCode).DscValue : null,
                                        val_max_discount = loopDataPromo.ItemType == "CUSTOM" ?
                                            loopDataPromo.PromoRuleResult.FirstOrDefault(q => q.Item == loopItemCart.SkuCode).MaxValue : null
                                    };

                                    listItemProductPromo.Add(itemProductPromo);
                                }

                                listItemProductPromoDetailCalculate.Add(ItemProductPromoDetailCalculate);
                            }
                        }

                        //Save Data Promo
                        Promo promo = new()
                        {
                            Code = loopDataPromo.Id,
                            Name = loopDataPromo.Name,
                            Type = loopDataPromo.PromoActionType,
                            Cls = loopDataPromo.Cls,
                            Lvl = loopDataPromo.Lvl,
                            ValDiscount = loopDataPromo.ItemType == "ALL" ? loopDataPromo.PromoActionValue : null,
                            ValMaxDiscount = loopDataPromo.ItemType == "ALL" ? loopDataPromo.MaxValue : null,
                            Rounding = loopDataPromo.PromoActionType.ToUpper() == "AMOUNT" && (loopDataPromo.Cls > 1 || cekHaveMaxDiscount) ?
                                        Convert.ToInt32(loopDataPromo.PromoActionValue) - Convert.ToInt32(totalDiscountAmount) : 0,
                            ItemPromo = listItemProductPromo
                        };

                        listPromo.Add(promo);
                    }

                    //Change price base with price after discount next class
                    foreach (var loopPirceBase in listItemProductPromoDetailCalculate)
                    {
                        if (loopPirceBase.TotalDiscount > 0)
                        {
                            decimal priceforchange = loopPirceBase.PriceTemp == 0 ? loopPirceBase.Price : loopPirceBase.PriceTemp;
                            loopPirceBase.PriceTemp = priceforchange - loopPirceBase.TotalDiscount / loopPirceBase.Qty;
                        }
                        else
                        {
                            loopPirceBase.PriceTemp = loopPirceBase.Price;
                        }
                    }

                    cekClassforPrice++;
                }

                decimal total_before = 0;
                decimal total_discount = 0;
                decimal total_after = 0;
                decimal total_rounding = 0;

                //Move Model Calculate to Model Response
                foreach (var loopGetTotal in listItemProductPromoDetailCalculate)
                {

                    ItemProductPromoDetail itemProductPromoDetail = new()
                    {
                        SkuCode = loopGetTotal.SkuCode,
                        SkuGroup = loopGetTotal.SkuGroup,
                        Qty = loopGetTotal.Qty,
                        Price = loopGetTotal.Price,
                        TotalBefore = loopGetTotal.TotalBefore,
                        TotalDiscount = loopGetTotal.TotalDiscount,
                        TotalAfter = loopGetTotal.TotalAfter
                    };

                    total_before += loopGetTotal.TotalBefore;
                    total_discount += loopGetTotal.TotalDiscount;
                    total_after += loopGetTotal.TotalAfter;

                    listItemProductPromoDetail.Add(itemProductPromoDetail);
                }

                foreach (var loopGetRounding in listPromo)
                {
                    total_rounding += loopGetRounding.Rounding;
                }

                CalculatePromoResponseDto dataPromoResponse = new()
                {
                    TotalBefore = total_before,
                    TotalDiscount = total_discount + total_rounding,
                    TotalAfter = total_after - total_rounding,
                    TotalRounding = Convert.ToInt32(total_rounding),
                    Promo = listPromo,
                    ItemProducts = listItemProductPromoDetail.OrderBy(q => q.SkuCode).ToList()
                };

                response.Data = dataPromoResponse;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        private static IEnumerable<IEnumerable<T>> CombineList<T>(IEnumerable<IEnumerable<T>> sequences)
        {
            IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };
            return sequences.Aggregate(
                emptyProduct,
                (accumulator, sequence) =>
                    from accseq in accumulator
                    from item in sequence
                    select accseq.Concat(new[] { item }));
        }
    }
}
