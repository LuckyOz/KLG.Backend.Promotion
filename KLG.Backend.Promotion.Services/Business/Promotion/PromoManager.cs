
using System.Text;
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

        public async Task<string[]> GetWorkflow()
        {
            List<string> promoWorkflowList = new();

            List<PromoWorkflow> listPromoWorkflow = await _dbProvider.DbContext.PromoWorkflow
                .Where(q => q.ActiveFlag)
                .Include(q => q.PromoWorkflowExpression)
                .ToListAsync();

            if (listPromoWorkflow.Count > 0) {

                foreach (var promoWorkflowHeader in listPromoWorkflow) {

                    //Add Global Params
                    List<GlobalParam> listGlobalParams = new();

                    if (promoWorkflowHeader.PromoWorkflowExpression.Count > 0) {

                        foreach (var loopGlobalParams in promoWorkflowHeader.PromoWorkflowExpression) {
                            GlobalParam GlobalParams = new()
                            {
                                Name = loopGlobalParams.Code,
                                Expression = loopGlobalParams.Expression
                            };

                            listGlobalParams.Add(GlobalParams);
                        }
                    }

                    //get data rule table
                    List<PromoRule> listPromoRule = _dbProvider.DbContext.PromoRule
                        .Where(q => q.PromoworkflowId == promoWorkflowHeader.Id)
                        .Include(q => q.PromoRuleVariable)
                        .Include(q => q.PromoRuleExpression)
                        .Include(q => q.PromoRuleResult)
                        .ToList();

                    //Add Rule 
                    if (listPromoRule.Count > 0) {
                        List<Rule> listRules = new();

                        foreach (var loopRules in listPromoRule) {
                            StringBuilder ruleExp = new();
                            int groupLine = 0, countGroupLine = 0;

                            List<LocalParam> listLocalParams = new();

                            //Add Local Params
                            //Add Local Params Variable
                            if (loopRules.PromoRuleVariable.Count > 0) {
                                foreach (var loopLocalParamsVar in loopRules.PromoRuleVariable) {
                                    LocalParam localParamas = new()
                                    {
                                        Name = loopLocalParamsVar.Code,
                                        Expression = loopLocalParamsVar.ParamsExpression
                                    };

                                    listLocalParams.Add(localParamas);
                                }
                            }

                            //Add Local Params Expresion
                            if (loopRules.PromoRuleExpression.Count > 0) {
                                foreach (var loopLocalParamsExp in loopRules.PromoRuleExpression) {
                                    LocalParam localParamas = new()
                                    {
                                        Name = loopLocalParamsExp.Code,
                                        Expression = loopLocalParamsExp.ParamsExpression
                                    };

                                    listLocalParams.Add(localParamas);

                                    var linkExp = loopLocalParamsExp.Linkexp != null &&
                                                    loopLocalParamsExp.Linkexp != "" ? " " + loopLocalParamsExp.Linkexp + " " : "";

                                    if (loopLocalParamsExp.Groupline != 0) {
                                        if (loopLocalParamsExp.Groupline > groupLine) {
                                            countGroupLine = loopRules.PromoRuleExpression
                                                                .Count(q => q.Groupline == loopLocalParamsExp.Groupline);
                                            ruleExp.Append("(" + loopLocalParamsExp.Code + linkExp);
                                            groupLine++;
                                            countGroupLine--;
                                        } else {
                                            countGroupLine--;
                                            if (countGroupLine == 0) {
                                                ruleExp.Append(loopLocalParamsExp.Code + ")" + linkExp);
                                            } else {
                                                ruleExp.Append(loopLocalParamsExp.Code + linkExp);
                                            }
                                        }
                                    } else {
                                        ruleExp.Append(loopLocalParamsExp.Code + linkExp);
                                    }
                                }
                            }

                            PromoRule getDataResult = loopRules;
                            getDataResult.PromoRuleVariable = null;
                            getDataResult.PromoRuleExpression = null;
                            getDataResult.PromoWorkflow = null;

                            foreach (var loopResult in getDataResult.PromoRuleResult) {
                                loopResult.PromoRule = null;
                            }

                            Context addContext = new()
                            {
                                DataPromo = getDataResult
                            };

                            OnSuccess addOnsuccess = new()
                            {
                                Name = "ResultPromo",
                                Context = addContext
                            };

                            Actions addAction = new()
                            {
                                OnSuccess = addOnsuccess
                            };

                            Rule rules = new()
                            {
                                Actions = addAction,
                                RuleName = loopRules.Id,
                                SuccessEvent = Convert.ToString(loopRules.Cls) + "#" + Convert.ToString(loopRules.Lvl),
                                LocalParams = listLocalParams,
                                Expression = ruleExp.ToString(),
                            };

                            listRules.Add(rules);
                        }

                        //Add Workflow  
                        RuleWorkflow ruleWorkflow = new()
                        {
                            WorkflowName = promoWorkflowHeader.Id,
                            GlobalParams = listGlobalParams,
                            Rules = listRules
                        };

                        string convertToString = JsonConvert.SerializeObject(ruleWorkflow);
                        promoWorkflowList.Add(convertToString);
                    }
                }
            }
            string[] workflowRules = promoWorkflowList.ToArray();
            return workflowRules;
        }

        public async Task<ServiceResponse<List<FindPromoResponseDto>>> FindPromo(PromoRequestDto promoRequestDto)
        {
            List<FindPromoResponseDto> listPromoResult = new();
            ServiceResponse<List<FindPromoResponseDto>> response = new();

            try {
                var promoResult = await _promoSetup.GetPromo(promoRequestDto.PromoType, promoRequestDto);
                promoResult = promoResult.Where(q => q.IsSuccess).ToList();

                foreach (var loopPromoResult in promoResult) {
                    var dataResultDetailString = JsonConvert.SerializeObject(loopPromoResult.ActionResult.Output);
                    var dataResultDetail = JsonConvert.DeserializeObject<FindPromoResponseDto>(dataResultDetailString);

                    if (dataResultDetail.Code != null) {
                        listPromoResult.Add(dataResultDetail);
                    }
                }

                if (listPromoResult.Count < 1) {
                    response.Success = false;
                    response.Message = "No Have Promo for This Cart";

                    return response;
                }

                response.Data = listPromoResult;

                return response;

            } catch (Exception ex) {
                response.Success = false;
                response.Message = ex.Message;

                return response;
            }
            
        }

        public async Task<ServiceResponse<ValidatePromoResponseDto>> ValidatePromo(PromoRequestDto promoRequestDto)
        {
            List<ItemProductValidate> listDataItemValidate = new();
            ServiceResponse<ValidatePromoResponseDto> response = new();

            try {
                var responseFind = await this.FindPromo(promoRequestDto);

                if (responseFind.Success && responseFind.Data.Count > 0) {

                    //Variable for save promo
                    List<FindPromoResponseDto> promoListDataSave = new();

                    //get list class for looping
                    var getListClass = responseFind.Data.Select(q => q.Cls).Distinct().ToList();

                    //looping class
                    foreach (var loopListClass in getListClass) {

                        //get data promo bedasarkan class
                        var cekPromoAllItem = responseFind.Data.Where(q => q.Cls == loopListClass && q.TypeResult == "ALL").ToList(); //get data promo untuk apply all item
                        var cekPromoCustomItem = responseFind.Data.Where(q => q.Cls == loopListClass && q.TypeResult == "CUSTOM").ToList(); //get data promo untuk apply custom item

                        if (cekPromoAllItem.Count > 0) {

                            FindPromoResponseDto promoDataSaveAll;

                            if (cekPromoAllItem.Count == 1) { //cek jika hanya terdapat 1 promo untuk apply all item
                                promoDataSaveAll = cekPromoAllItem.FirstOrDefault();
                            } else {
                                //jika promo untuk apply all item lebih dari 1
                                if (cekPromoAllItem.Any(q => q.Lvl == 1)) {
                                    //menangin promo apply all item untuk result get item
                                    //order promo untuk dapat promo yang terbesar
                                    promoDataSaveAll = cekPromoAllItem.Where(q => q.Lvl == 1).OrderByDescending(q => q.PromoListItem
                                                                   .Sum(q => q.TotalDiscount)).FirstOrDefault();
                                } else {
                                    //promo apply all item result selain get item
                                    //order promo untuk dapat promo yang terbesar
                                    promoDataSaveAll = cekPromoAllItem.OrderByDescending(q => q.PromoListItem
                                                                   .Sum(q => q.TotalDiscount)).FirstOrDefault();
                                }
                            }

                            promoListDataSave.Add(promoDataSaveAll);

                        } else {//cek jika terdapat promo apply custom item dan tidak ada apply all item

                            if (cekPromoCustomItem.Count == 1) {
                                //jika hanya terdapat 1 promo untuk apply custom item

                                FindPromoResponseDto promoDataSaveCustom;

                                if (cekPromoCustomItem[0].PromoListItem.Count == 1) {
                                    //jika hanya terdapat 1 kombinasi di 1 promo untuk apply custom item
                                    promoDataSaveCustom = cekPromoCustomItem[0];

                                } else {

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
                            } else {
                                //jika terdapat lebih dari 1 promo untuk apply custom item

                                //Looping untuk mendapatkan kombinasi jumlah item

                                List<List<int>> listlistItemPromoCombination = new();

                                foreach (var loopPromoCustomItem in cekPromoCustomItem) {
                                    List<int> listItemPromoCombination = new();

                                    for (int i = 0; i < loopPromoCustomItem.PromoListItem.Count; i++) {
                                        listItemPromoCombination.Add(i);
                                    }

                                    listlistItemPromoCombination.Add(listItemPromoCombination);
                                }

                                var resultCombi = CombineList(listlistItemPromoCombination);

                                //looping untuk implementasi jumlah item ke item dengan discountnya
                                List<List<FindPromoResponseDto>> listListNewDataPromoTemp = new();

                                foreach (var loopResultCombi in resultCombi) {
                                    int count = 0;

                                    List<FindPromoResponseDto> listNewDataPromoTemp = new();
                                    List<FindPromoResponseDto> listNewDataPromoTempDelete = new();

                                    foreach (var loopLoopResultCombi in loopResultCombi) {

                                        FindPromoResponseDto newDataPromoTemp = new(cekPromoCustomItem[count])
                                        {
                                            PromoListItem = new()
                                            {
                                                new PromoListItem(cekPromoCustomItem[count].PromoListItem[loopLoopResultCombi])
                                            }
                                        };

                                        bool cekInsert = true;

                                        foreach (var loopCekItemSameGroup in newDataPromoTemp.PromoListItem) {
                                            foreach (var loopCekItemSameHead in loopCekItemSameGroup.PromoListItemDetail) {
                                                foreach (var loopItemSameGroupHasAdd in listNewDataPromoTemp) {
                                                    foreach (var loopCekItemSameHeadHasAdd in loopItemSameGroupHasAdd.PromoListItem) {
                                                        foreach (var loopItemSameDetailHasAdd in loopCekItemSameHeadHasAdd.PromoListItemDetail) {
                                                            if (loopCekItemSameHead.SkuCode == loopItemSameDetailHasAdd.SkuCode) {

                                                                decimal itemDisc01 = 0, itemDisc02 = 0;

                                                                if(loopItemSameDetailHasAdd.ValDiscount == "FREE") {
                                                                    itemDisc01 = Convert.ToDecimal(loopItemSameDetailHasAdd.Price);
                                                                } else {
                                                                    itemDisc01 = loopItemSameDetailHasAdd.ValDiscount.Contains('%') ?
                                                                        Convert.ToDecimal(loopItemSameDetailHasAdd.ValDiscount.Replace("%", "")) :
                                                                        Convert.ToDecimal(loopItemSameDetailHasAdd.ValDiscount);
                                                                }

                                                                if(loopCekItemSameHead.ValDiscount == "FREE") {
                                                                    itemDisc02 = Convert.ToDecimal(loopCekItemSameHead.Price);
                                                                } else {
                                                                    itemDisc02 = loopCekItemSameHead.ValDiscount.Contains('%') ?
                                                                        Convert.ToDecimal(loopCekItemSameHead.ValDiscount.Replace("%", "")) :
                                                                        Convert.ToDecimal(loopCekItemSameHead.ValDiscount);
                                                                }

                                                                if (itemDisc01 >= itemDisc02) {
                                                                    cekInsert = false;
                                                                } else {
                                                                    listNewDataPromoTempDelete.Add(loopItemSameGroupHasAdd);
                                                                }

                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        if (listNewDataPromoTempDelete.Count > 0) {
                                            foreach (var loopDeleteList in listNewDataPromoTempDelete) {
                                                listNewDataPromoTemp.Remove(loopDeleteList);
                                            }
                                        }

                                        if (cekInsert) {
                                            listNewDataPromoTemp.Add(newDataPromoTemp);
                                        }

                                        count++;
                                    }

                                    listListNewDataPromoTemp.Add(listNewDataPromoTemp);
                                }

                                //Get Promo Tertinggi
                                int countSum = 0;
                                Dictionary<int, double> cekListSum = new();

                                foreach (var loopGetPromoOrder in listListNewDataPromoTemp) {
                                    double sumPromo = 0;

                                    foreach (var loopSumCek in loopGetPromoOrder) {
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

                    foreach (var loopItemRequest in promoRequestDto.ItemProduct) {
                        
                        List<PromoProductValidate> listDataPromoforItem = new();

                        foreach (var loopDataProtmoList in promoListDataSave) {

                            if (loopDataProtmoList.PromoListItem.Count < 1) {
                                continue;
                            }

                            if (loopDataProtmoList.PromoListItem[0].PromoListItemDetail.Count < 1) {
                                continue;
                            }

                            var dataPromoPerItem = loopDataProtmoList.PromoListItem[0]
                                                    .PromoListItemDetail
                                                    .FirstOrDefault(q => q.SkuCode == loopItemRequest.SkuCode);

                            if (dataPromoPerItem != null) {

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

                } else {

                    foreach (var loopItemRequest in promoRequestDto.ItemProduct) {
                        
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

            } catch (Exception ex) {
                response.Success = false;
                response.Message = ex.Message;

                return response;
            }
        }

        public async Task<ServiceResponse<CalculatePromoResponseDto>> CalculatePromo(PromoRequestDto promoRequestDto, List<string> listmaxDiscount)
        {
            CalculatePromoResponseDto dataPromoResponse = new();
            ServiceResponse<CalculatePromoResponseDto> response = new();

            try {
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

                foreach (var loopListClass in listClass) {

                    //Untuk order get item terlebih dahulu
                    var listDataPromoClass = listDataPromo.Where(q => q.Cls == loopListClass).OrderBy(q => q.Lvl).ToList();

                    //Looping discount apply
                    foreach (var loopDataPromo in listDataPromoClass) {

                        bool cekHaveMaxDiscount = false;

                        if (listmaxDiscount.Any(q => q == loopDataPromo.Id)) {
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
                        if (loopDataPromo.ItemType == "CUSTOM") {
                            foreach (var loopItemResult in loopDataPromo.PromoRuleResult) {
                                listItemResult.Add(loopItemResult.Item);
                            }
                        }

                        //Execute Discount
                        if (listItemProductPromoDetailCalculate.Count > 0) {
                            //Looping untuk execute discount yang sudah pernah dilakukan sebelumnya menggunakan model discount
                            foreach (var loopItemModelItemDiscount in listItemProductPromoDetailCalculate) {
                                //Variable for Save Discount
                                decimal discount = 0;

                                //Setup Price , Cek if anotherclass using price temp
                                decimal usingPrice = cekClassforPrice > 0 ? loopItemModelItemDiscount.PriceTemp : loopItemModelItemDiscount.Price;

                                //Execute Discount Percent
                                if (loopDataPromo.PromoActionType == "PERCENT") {

                                    if (loopDataPromo.ItemType == "ALL") {

                                        //Execute Discount Percent to All Item.
                                        discount = (usingPrice * loopItemModelItemDiscount.Qty)
                                                    * Convert.ToDecimal(loopDataPromo.PromoActionValue.Replace("%", "")) / 100;

                                    }

                                    if (loopDataPromo.ItemType == "CUSTOM") {

                                        //Cek Item Avaible to Calculate Discount
                                        if (listItemResult.Any(q => q.Contains(loopItemModelItemDiscount.SkuCode))) {

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
                                }

                                //Execute Discount Amount 
                                if (loopDataPromo.PromoActionType == "AMOUNT") {

                                    //Cek Promo di Class Item Atau Cart
                                    if (loopDataPromo.Cls == 1 && !cekHaveMaxDiscount) {
                                        //Promo Amount Class 1 / Level Item di apply hanya di Item tidak di prorate ke semua item cart

                                        //Cek Promo Type Item ke All Item atau Custom
                                        if (loopDataPromo.ItemType == "ALL") {

                                            //Cek Jika Max Value (Kelipatan Discount) Lebih dari Qty atau tidak atau 0
                                            if (loopItemModelItemDiscount.Qty > Convert.ToInt32(loopDataPromo.MaxValue)
                                                    || Convert.ToInt32(loopDataPromo.MaxValue) == 0 || loopDataPromo.MaxValue is null) {

                                                //Execute Discount Amount
                                                discount = Convert.ToDecimal(loopDataPromo.PromoActionValue) * loopItemModelItemDiscount.Qty;

                                            } else if (Convert.ToInt32(loopDataPromo.MaxValue) > loopItemModelItemDiscount.Qty) {

                                                //Execute Discount Amount
                                                discount = Convert.ToDecimal(loopDataPromo.PromoActionValue) * Convert.ToDecimal(loopDataPromo.MaxValue);
                                            }

                                        }

                                        //Cek Promo Amount Jika Custom Item
                                        if (loopDataPromo.ItemType == "CUSTOM") {

                                            //Cek Item Cart Termasuk ke Dalam Item Promo Atau Tidak
                                            if (listItemResult.Any(q => q.Contains(loopItemModelItemDiscount.SkuCode))) {

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
                                                    || Convert.ToInt32(discMaxValue) == 0 || discMaxValue is null) {

                                                    discount = Convert.ToDecimal(discValueRslt) * loopItemModelItemDiscount.Qty;

                                                } else if (Convert.ToInt32(discMaxValue) < loopItemModelItemDiscount.Qty) { //jika max value lebih dari qty
                                                                                                                            // Execute Discount Amount
                                                    discount = Convert.ToDecimal(discValueRslt) * Convert.ToDecimal(discMaxValue);
                                                }

                                            }

                                        }

                                    } else {
                                        //Exec Promo Amount ada di class 2 atau di level cart akan di prorate ke semua item yang ada di cart

                                        //Untuk Calculate Total Semua Item Promo
                                        decimal totalPriceItemCart = 0;

                                        //Untuk Get Total Item Promo 
                                        decimal totalPriceSku = promoRequestDto.ItemProduct.Where(q => q.SkuCode == loopItemModelItemDiscount.SkuCode)
                                                                                            .Sum(q => q.Qty * q.Price);

                                        if (loopDataPromo.ItemType == "ALL") {

                                            //All Total Item at Cart
                                            totalPriceItemCart = promoRequestDto.ItemProduct.Sum(q => q.Price * q.Qty);
                                            discount = (totalPriceSku / totalPriceItemCart) * Convert.ToDecimal(loopDataPromo.PromoActionValue);

                                        }

                                        if (loopDataPromo.ItemType == "CUSTOM") {

                                            //Custom Total Item at Cart
                                            if (listItemResult.Any(q => q.Contains(loopItemModelItemDiscount.SkuCode))) {

                                                totalPriceItemCart = promoRequestDto.ItemProduct.Where(q => listItemResult
                                                                    .Contains(q.SkuCode)).Sum(q => q.Price * q.Qty);

                                                discount = (totalPriceSku / totalPriceItemCart) * Convert.ToDecimal(loopDataPromo.PromoActionValue);
                                            }

                                        }

                                        totalDiscountAmount += Convert.ToInt32(Math.Floor(discount));
                                    }
                                }

                                //Execute Discount Item
                                if (loopDataPromo.PromoActionType == "ITEM") {

                                    if (listItemResult.Any(q => q.Contains(loopItemModelItemDiscount.SkuCode))) {

                                        int maxValue = Convert.ToInt32(loopDataPromo.PromoRuleResult
                                                                .Where(q => q.Item == loopItemModelItemDiscount.SkuCode)
                                                                .Select(q => q.MaxValue).FirstOrDefault());

                                        maxValue = maxValue > loopItemModelItemDiscount.Qty ? loopItemModelItemDiscount.Qty : maxValue;

                                        discount = loopItemModelItemDiscount.Price * maxValue;
                                    }

                                }

                                loopItemModelItemDiscount.TotalDiscount += Convert.ToInt32(Math.Floor(discount));
                                loopItemModelItemDiscount.TotalAfter = loopItemModelItemDiscount.TotalBefore - loopItemModelItemDiscount.TotalDiscount;

                                //Save Data Promo to Model
                                ItemProductPromo itemProductPromo = new();
                                itemProductPromo.sku_code = loopItemModelItemDiscount.SkuCode;
                                itemProductPromo.discount = Convert.ToInt32(Math.Floor(discount));

                                if (itemProductPromo.discount > 0) {
                                    listItemProductPromo.Add(itemProductPromo);
                                }
                            }

                        } else {
                            //Looping untuk execute discount pertama kali dan menggunkan item dari cart dan hasilnya di masukan ke model discount
                            foreach (var loopItemCart in promoRequestDto.ItemProduct) {

                                //Variable for Save Discount
                                decimal discount = 0;

                                //Model for Save Detail Discount
                                ItemProductPromoDetailCalculate ItemProductPromoDetailCalculate = new();

                                //Execute Discount Percent
                                if (loopDataPromo.PromoActionType == "PERCENT") {

                                    if (loopDataPromo.ItemType == "ALL") {

                                        //Execute Discount Percent to All Item.
                                        discount = (loopItemCart.Price * loopItemCart.Qty)
                                                    * Convert.ToDecimal(loopDataPromo.PromoActionValue.Replace("%", "")) / 100;

                                    } else if (loopDataPromo.ItemType == "CUSTOM") {

                                        //Cek Item Avaible to Calculate Discount
                                        if (listItemResult.Any(q => q.Contains(loopItemCart.SkuCode))) {

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

                                }

                                //Execute Discount Amount
                                if (loopDataPromo.PromoActionType == "AMOUNT") {

                                    //Cek Promo di Class Item Atau Cart
                                    if (loopDataPromo.Cls == 1 && !cekHaveMaxDiscount) {

                                        //Promo Amount Class 1 / Level Item di apply hanya di Item tidak di prorate ke semua item cart

                                        //Cek Promo Type Item ke All Item atau Custom
                                        if (loopDataPromo.ItemType == "ALL") {

                                            //Cek Jika Max Value (Kelipatan Discount) Lebih dari Qty atau tidak atau 0
                                            if (loopItemCart.Qty > Convert.ToInt32(loopDataPromo.MaxValue)
                                                    || Convert.ToInt32(loopDataPromo.MaxValue) == 0 || loopDataPromo.MaxValue is null) {

                                                //Execute Discount Amount
                                                discount = Convert.ToDecimal(loopDataPromo.PromoActionValue) * loopItemCart.Qty;

                                            } else if (Convert.ToInt32(loopDataPromo.MaxValue) > loopItemCart.Qty) {

                                                //Execute Discount Amount
                                                discount = Convert.ToDecimal(loopDataPromo.PromoActionValue) * Convert.ToDecimal(loopDataPromo.MaxValue);
                                            }

                                        } else if (loopDataPromo.itemtype == "CUSTOM") { //Cek Promo Amount Jika Custom Item

                                            //Cek Item Cart Termasuk ke Dalam Item Promo Atau Tidak
                                            if (listItemResult.Any(q => q.Contains(loopItemCart.sku_code))) {

                                                //Get Discount Amount Value in Rslt List
                                                string discValueRslt = loopDataPromo.md_promo_rule_rsls
                                                                    .Where(q => q.item == loopItemCart.sku_code)
                                                                    .Select(q => q.dscvalue)
                                                                    .FirstOrDefault();

                                                string discMacValue = loopDataPromo.md_promo_rule_rsls
                                                                    .Where(q => q.item == loopItemCart.sku_code)
                                                                    .Select(q => q.maxvalue)
                                                                    .FirstOrDefault();

                                                //Cek Value max value termasuk lebih dari qty / infinite base on qty / null
                                                if (Convert.ToInt32(discMacValue) > loopItemCart.qty
                                                    || Convert.ToInt32(discMacValue) == 0 || discMacValue is null) {

                                                    discount = Convert.ToDecimal(discValueRslt) * loopItemCart.qty;

                                                } else if (Convert.ToInt32(discMacValue) < loopItemCart.qty) { //jika max value lebih dari qty
                                                                                                               // Execute Discount Amount
                                                    discount = Convert.ToDecimal(discValueRslt) * Convert.ToDecimal(discMacValue);
                                                }

                                            }

                                        }

                                    } else {

                                        //Exec Promo Amount ada di class 2 atau di level cart akan di prorate ke semua item yang ada di cart

                                        //Untuk Calculate Total Semua Item Promo
                                        decimal totalPriceItemCart = 0;

                                        //Untuk Get Total Item Promo 
                                        decimal totalPriceSku = promoRequestDto.item_product.Where(q => q.sku_code == loopItemCart.sku_code).Sum(q => q.qty * q.price);

                                        if (loopDataPromo.itemtype == "ALL") {

                                            //All Total Item at Cart
                                            totalPriceItemCart = promoRequestDto.item_product.Sum(q => q.price * q.qty);
                                            discount = (totalPriceSku / totalPriceItemCart) * Convert.ToDecimal(loopDataPromo.promoactionvalue);

                                        } else if (loopDataPromo.itemtype == "CUSTOM") {

                                            //Custom Total Item at Cart
                                            if (listItemResult.Any(q => q.Contains(loopItemCart.sku_code))) {

                                                totalPriceItemCart = promoRequestDto.item_product.Where(q => listItemResult
                                                                    .Contains(q.sku_code)).Sum(q => q.price * q.qty);

                                                discount = (totalPriceSku / totalPriceItemCart) * Convert.ToDecimal(loopDataPromo.promoactionvalue);
                                            }

                                        }

                                        totalDiscountAmount += Convert.ToInt32(Math.Floor(discount));
                                    }

                                }

                                //Execute Discount Item
                                if (loopDataPromo.promoactiontype == "ITEM") {

                                    if (listItemResult.Any(q => q.Contains(loopItemCart.sku_code))) {

                                        int maxValue = Convert.ToInt32(loopDataPromo.md_promo_rule_rsls
                                                                .Where(q => q.item == loopItemCart.sku_code)
                                                                .Select(q => q.maxvalue).FirstOrDefault());

                                        maxValue = maxValue > loopItemCart.qty ? loopItemCart.qty : maxValue;

                                        discount = loopItemCart.price * maxValue;
                                    }

                                }

                                //Save Data Item Execute Promo to Model Promo Response
                                ItemProductPromoDetailCalculate.sku_code = loopItemCart.sku_code;
                                ItemProductPromoDetailCalculate.sku_group = loopItemCart.sku_group;
                                ItemProductPromoDetailCalculate.qty = loopItemCart.qty;
                                ItemProductPromoDetailCalculate.price = loopItemCart.price;
                                ItemProductPromoDetailCalculate.total_before = ItemProductPromoDetailCalculate.qty * ItemProductPromoDetailCalculate.price;
                                ItemProductPromoDetailCalculate.total_discount = Convert.ToInt32(Math.Floor(discount));
                                ItemProductPromoDetailCalculate.total_after = ItemProductPromoDetailCalculate.total_before - ItemProductPromoDetailCalculate.total_discount;

                                item_product_promo itemProductPromo = new item_product_promo();
                                itemProductPromo.sku_code = loopItemCart.sku_code;
                                itemProductPromo.discount = Convert.ToInt32(Math.Floor(discount));

                                if (itemProductPromo.discount > 0) {
                                    if (loopDataPromo.itemtype == "CUSTOM") {
                                        var dataDiscountperItem = loopDataPromo.md_promo_rule_rsls.Where(q => q.item == loopItemCart.sku_code)
                                                                                                  .FirstOrDefault();
                                        itemProductPromo.val_discount = dataDiscountperItem.dscvalue;
                                        itemProductPromo.val_max_discount = dataDiscountperItem.maxvalue;
                                    }

                                    listItemProductPromo.Add(itemProductPromo);
                                }

                                listItemProductPromoDetailCalculate.Add(ItemProductPromoDetailCalculate);
                            }
                        }

                        promo promo = new promo();
                        promo.code = loopDataPromo.rulecode;
                        promo.name = loopDataPromo.rulename;
                        promo.type = loopDataPromo.promoactiontype;
                        promo.cls = loopDataPromo.cls;
                        promo.lvl = loopDataPromo.lvl;

                        if (loopDataPromo.itemtype == "ALL") {
                            promo.val_discount = loopDataPromo.promoactionvalue;
                            promo.val_max_discount = loopDataPromo.maxvalue;
                        }

                        //Get Rounding For Promo Detail
                        promo.rounding = loopDataPromo.promoactiontype.ToUpper() == "AMOUNT" && (loopDataPromo.cls > 1 || cekHaveMaxDiscount) ?
                                            Convert.ToInt32(loopDataPromo.promoactionvalue) - Convert.ToInt32(totalDiscountAmount) : 0;

                        //Save Data Promo
                        promo.itemPromo = listItemProductPromo;

                        listPromo.Add(promo);
                    }

                    //Change price base with price after discount next class
                    foreach (var loopPirceBase in listItemProductPromoDetailCalculate) {
                        if (loopPirceBase.total_discount > 0) {
                            decimal priceforchange = loopPirceBase.price_temp == 0 ? loopPirceBase.price : loopPirceBase.price_temp;
                            loopPirceBase.price_temp = priceforchange - loopPirceBase.total_discount / loopPirceBase.qty;
                        } else {
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
                foreach (var loopGetTotal in listItemProductPromoDetailCalculate) {
                    ItemProductPromoDetail itemProductPromoDetail = new();

                    itemProductPromoDetail.SkuCode = loopGetTotal.SkuCode;
                    itemProductPromoDetail.SkuGroup = loopGetTotal.SkuGroup;
                    itemProductPromoDetail.Qty = loopGetTotal.Qty;
                    itemProductPromoDetail.Price = loopGetTotal.Price;
                    itemProductPromoDetail.TotalBefore = loopGetTotal.TotalBefore;
                    itemProductPromoDetail.TotalDiscount = loopGetTotal.TotalDiscount;
                    itemProductPromoDetail.TotalAfter = loopGetTotal.TotalAfter;

                    total_before += loopGetTotal.TotalBefore;
                    total_discount += loopGetTotal.TotalDiscount;
                    total_after += loopGetTotal.TotalAfter;

                    listItemProductPromoDetail.Add(itemProductPromoDetail);
                }

                foreach (var loopGetRounding in listPromo) {
                    total_rounding += loopGetRounding.Rounding;
                }

                dataPromoResponse.TotalBefore = total_before;
                dataPromoResponse.TotalDiscount = total_discount + total_rounding;
                dataPromoResponse.TotalAfter = total_after - total_rounding;
                dataPromoResponse.TotalRounding = Convert.ToInt32(total_rounding);
                dataPromoResponse.Promo = listPromo;
                dataPromoResponse.ItemProducts = listItemProductPromoDetail.OrderBy(q => q.SkuCode).ToList();

                response.Data = dataPromoResponse;

            } catch (Exception ex) {
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
