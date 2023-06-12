
using KLG.Backend.Promotion.Services.Entities;

namespace KLG.Backend.Promotion.Services.Resources
{
    public class DbTesting
    {
        public List<PromoWorkflow> listmspromoworkflow = new()
        {
            new PromoWorkflow
            {
                Id = "ed95cc28-d408-4d3c-9736-3024c7a4f8f4",
                Name = "CHATIME PROMO",
                ActiveFlag = true,
                PromoWorkflowExpression = new List<PromoWorkflowExpression>()
                {
                    //Var Total Price in Cart
                    new PromoWorkflowExpression
                    {
                        Id = Guid.NewGuid().ToString(),
                        Code = "total_price",
                        Name = "Total Price Item",
                        Expression = "Convert.ToDecimal(paramsPromo.ItemProduct.Sum(y => y.price * y.qty).ToString())",
                        ActiveFlag = true
                    },
                    //var Total Qty in Cart
                    new PromoWorkflowExpression
                    {
                        Id = Guid.NewGuid().ToString(),
                        Code = "total_qty",
                        Name = "Total Qty Item",
                        Expression = "Convert.ToDecimal(paramsPromo.ItemProduct.Sum(y => y.qty).ToString())",
                        ActiveFlag = true
                    }
                },
                PromoRule = new List<PromoRule>
                {
                    //Promo Amount Class Cart All Item
                    new PromoRule
                    {
                        Id = "2c4ce78f-f8e8-4dc5-ae0a-06c8423a966d",
                        Name = "Promo Happy New Year",
                        Cls = 2,
                        Lvl = 2,
                        ItemType = "ALL",
                        ResultType = "V1",
                        PromoActionType = "AMOUNT",
                        PromoActionValue = "5000",
                        MaxValue = "",
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddYears(1),
                        ActiveFlag = true,

                        PromoRuleVariable = new List<PromoRuleVariable>()
                        {
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Code = "startDate",
                                Name = "Start Date",
                                ParamsExpression = "Convert.ToDateTime(\"25-12-2022\")",
                                ActiveFlag = true
                            },
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Code = "endDate",
                                Name = "End Date",
                                ParamsExpression = "Convert.ToDateTime(\"31-12-2022\")",
                                ActiveFlag = true
                            },
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Code = "prmDate",
                                Name = "Params Date",
                                ParamsExpression = "Convert.ToDateTime(paramsPromo.TransDate)",
                                ActiveFlag = true
                            },
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 4,
                                Code = "zoneDisc",
                                Name = "Zone Discount",
                                ParamsExpression = "new string[]{\"A\",\"B\",\"C\",\"D\",\"E\",\"F\",\"G\",\"H\",\"I\"}",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleExpression = new List<PromoRuleExpression>()
                        {
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 0,
                                Code = "cekDateStart",
                                Name = "Cek Date Start",
                                ParamsExpression = "prmDate >= startDate",
                                Linkexp = "AND",
                                ActiveFlag = true
                            },
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Groupline = 0,
                                Code = "cekDateEnd",
                                Name = "Cek Date End",
                                ParamsExpression = "endDate >= prmDate",
                                Linkexp = "AND",
                                ActiveFlag = true
                            },
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Groupline = 0,
                                Code = "cekZone",
                                Name = "Cek Zone Avail",
                                ParamsExpression = "!zoneDisc.Contains(paramsPromo.Zone)",
                                Linkexp = "",
                                ActiveFlag = true
                            }
                        }
                    },
                    //Promo Amount Class Cart Custom Item Result AND
                    new PromoRule
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Promo Happy New Year Custom Item AND",
                        Cls = 2,
                        Lvl = 2,
                        ItemType = "CUSTOM",
                        ResultType = "V1",
                        PromoActionType = "AMOUNT",
                        PromoActionValue = "10000",
                        MaxValue = "",
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddYears(1),
                        ActiveFlag = true,

                        PromoRuleVariable = new List<PromoRuleVariable>()
                        {
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Code = "startDate",
                                Name = "Start Date",
                                ParamsExpression = "Convert.ToDateTime(\"25-12-2022\")",
                                ActiveFlag = true
                            },
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Code = "endDate",
                                Name = "End Date",
                                ParamsExpression = "Convert.ToDateTime(\"31-12-2022\")",
                                ActiveFlag = true
                            },
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Code = "prmDate",
                                Name = "Params Date",
                                ParamsExpression = "Convert.ToDateTime(paramsPromo.TransDate)",
                                ActiveFlag = true
                            },
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 4,
                                Code = "zoneDisc",
                                Name = "Zone Discount",
                                ParamsExpression = "new string[]{\"A\",\"B\",\"C\"}",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleExpression = new List<PromoRuleExpression>()
                        {
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 0,
                                Code = "cekDateStart",
                                Name = "Cek Date Start",
                                ParamsExpression = "prmDate >= startDate",
                                Linkexp = "AND",
                                ActiveFlag = true
                            },
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Groupline = 0,
                                Code = "cekDateEnd",
                                Name = "Cek Date End",
                                ParamsExpression = "endDate >= prmDate",
                                Linkexp = "AND",
                                ActiveFlag = true
                            },
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Groupline = 0,
                                Code = "cekZone",
                                Name = "Cek Zone Avail",
                                ParamsExpression = "zoneDisc.Contains(paramsPromo.Zone)",
                                Linkexp = "",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleResult = new List<PromoRuleResult>()
                        {
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 0,
                                Linkrsl =  "AND",
                                Item = "tea21",
                                DscValue = "10000",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Groupline = 0,
                                Linkrsl =  "AND",
                                Item = "tea22",
                                DscValue = "10000",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Groupline = 0,
                                Linkrsl =  "AND",
                                Item = "tea23",
                                DscValue = "10000",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 4,
                                Groupline = 0,
                                Linkrsl =  "AND",
                                Item = "tea24",
                                DscValue = "10000",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 5,
                                Groupline = 0,
                                Linkrsl =  "",
                                Item = "tea25",
                                DscValue = "10000",
                                MaxValue = "",
                                ActiveFlag = true
                            }
                        }
                    },
                    //Promo Amount Class Cart Custom Item Result AND V2
                    new PromoRule
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Promo Happy New Year Custom Item AND V2",
                        Cls = 2,
                        Lvl = 2,
                        ItemType = "CUSTOM",
                        ResultType = "V2",
                        PromoActionType = "AMOUNT",
                        PromoActionValue = "10000",
                        MaxValue = "",
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddYears(1),
                        ActiveFlag = true,

                        PromoRuleVariable = new List<PromoRuleVariable>()
                        {
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Code = "startDate",
                                Name = "Start Date",
                                ParamsExpression = "Convert.ToDateTime(\"25-12-2022\")",
                                ActiveFlag = true
                            },
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Code = "endDate",
                                Name = "End Date",
                                ParamsExpression = "Convert.ToDateTime(\"31-12-2022\")",
                                ActiveFlag = true
                            },
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Code = "prmDate",
                                Name = "Params Date",
                                ParamsExpression = "Convert.ToDateTime(paramsPromo.TransDate)",
                                ActiveFlag = true
                            },
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 4,
                                Code = "zoneDisc",
                                Name = "Zone Discount",
                                ParamsExpression = "new string[]{\"A\",\"B\",\"C\"}",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleExpression = new List<PromoRuleExpression>()
                        {
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 0,
                                Code = "cekDateStart",
                                Name = "Cek Date Start",
                                ParamsExpression = "prmDate >= startDate",
                                Linkexp = "AND",
                                ActiveFlag = true
                            },
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Groupline = 0,
                                Code = "cekDateEnd",
                                Name = "Cek Date End",
                                ParamsExpression = "endDate >= prmDate",
                                Linkexp = "AND",
                                ActiveFlag = true
                            },
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Groupline = 0,
                                Code = "cekZone",
                                Name = "Cek Zone Avail",
                                ParamsExpression = "zoneDisc.Contains(paramsPromo.Zone)",
                                Linkexp = "",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleResult = new List<PromoRuleResult>()
                        {
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 0,
                                Linkrsl =  "AND",
                                Item = "tea21",
                                DscValue = "10000",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Groupline = 0,
                                Linkrsl =  "AND",
                                Item = "tea22",
                                DscValue = "10000",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Groupline = 0,
                                Linkrsl =  "AND",
                                Item = "tea23",
                                DscValue = "10000",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 4,
                                Groupline = 0,
                                Linkrsl =  "AND",
                                Item = "tea24",
                                DscValue = "10000",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 5,
                                Groupline = 0,
                                Linkrsl =  "",
                                Item = "tea25",
                                DscValue = "10000",
                                MaxValue = "",
                                ActiveFlag = true
                            }
                        }
                    },
                    //Promo Amount Class Cart Custom Item Result OR
                    new PromoRule
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Promo Happy New Year Custom Item OR",
                        Cls = 2,
                        Lvl = 2,
                        ItemType = "CUSTOM",
                        ResultType = "V1",
                        PromoActionType = "AMOUNT",
                        PromoActionValue = "10000",
                        MaxValue = "",
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddYears(1),
                        ActiveFlag = true,

                        PromoRuleVariable = new List<PromoRuleVariable>()
                        {
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Code = "startDate",
                                Name = "Start Date",
                                ParamsExpression = "Convert.ToDateTime(\"25-12-2022\")",
                                ActiveFlag = true
                            },
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Code = "endDate",
                                Name = "End Date",
                                ParamsExpression = "Convert.ToDateTime(\"31-12-2022\")",
                                ActiveFlag = true
                            },
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Code = "prmDate",
                                Name = "Params Date",
                                ParamsExpression = "Convert.ToDateTime(paramsPromo.TransDate)",
                                ActiveFlag = true
                            },
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 4,
                                Code = "zoneDisc",
                                Name = "Zone Discount",
                                ParamsExpression = "new string[]{\"D\",\"E\",\"F\"}",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleExpression = new List<PromoRuleExpression>()
                        {
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 0,
                                Code = "cekDateStart",
                                Name = "Cek Date Start",
                                ParamsExpression = "prmDate >= startDate",
                                Linkexp = "AND",
                                ActiveFlag = true
                            },
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Groupline = 0,
                                Code = "cekDateEnd",
                                Name = "Cek Date End",
                                ParamsExpression = "endDate >= prmDate",
                                Linkexp = "AND",
                                ActiveFlag = true
                            },
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Groupline = 0,
                                Code = "cekZone",
                                Name = "Cek Zone Avail",
                                ParamsExpression = "zoneDisc.Contains(paramsPromo.Zone)",
                                Linkexp = "",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleResult = new List<PromoRuleResult>()
                        {
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 1,
                                Linkrsl =  "OR",
                                Item = "tea21",
                                DscValue = "10000",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Groupline = 2,
                                Linkrsl =  "OR",
                                Item = "tea22",
                                DscValue = "10000",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Groupline = 3,
                                Linkrsl =  "OR",
                                Item = "tea23",
                                DscValue = "10000",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 4,
                                Groupline = 4,
                                Linkrsl =  "OR",
                                Item = "tea24",
                                DscValue = "10000",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 5,
                                Groupline = 5,
                                Linkrsl =  "",
                                Item = "tea25",
                                DscValue = "10000",
                                MaxValue = "",
                                ActiveFlag = true
                            }
                        }
                    },
                    //Promo Amount Class Cart Custom Item Result Combination AND OR
                    new PromoRule
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Promo Happy New Year Custom Item COMBI",
                        Cls = 2,
                        Lvl = 2,
                        ItemType = "CUSTOM",
                        ResultType = "V1",
                        PromoActionType = "AMOUNT",
                        PromoActionValue = "10000",
                        MaxValue = "",
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddYears(1),
                        ActiveFlag = true,

                        PromoRuleVariable = new List<PromoRuleVariable>()
                        {
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Code = "startDate",
                                Name = "Start Date",
                                ParamsExpression = "Convert.ToDateTime(\"25-12-2022\")",
                                ActiveFlag = true
                            },
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Code = "endDate",
                                Name = "End Date",
                                ParamsExpression = "Convert.ToDateTime(\"31-12-2022\")",
                                ActiveFlag = true
                            },
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Code = "prmDate",
                                Name = "Params Date",
                                ParamsExpression = "Convert.ToDateTime(paramsPromo.TransDate)",
                                ActiveFlag = true
                            },
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 4,
                                Code = "zoneDisc",
                                Name = "Zone Discount",
                                ParamsExpression = "new string[]{\"G\",\"H\",\"I\"}",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleExpression = new List<PromoRuleExpression>()
                        {
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 0,
                                Code = "cekDateStart",
                                Name = "Cek Date Start",
                                ParamsExpression = "prmDate >= startDate",
                                Linkexp = "AND",
                                ActiveFlag = true
                            },
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Groupline = 0,
                                Code = "cekDateEnd",
                                Name = "Cek Date End",
                                ParamsExpression = "endDate >= prmDate",
                                Linkexp = "AND",
                                ActiveFlag = true
                            },
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Groupline = 0,
                                Code = "cekZone",
                                Name = "Cek Zone Avail",
                                ParamsExpression = "zoneDisc.Contains(paramsPromo.Zone)",
                                Linkexp = "",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleResult = new List<PromoRuleResult>()
                        {
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 1,
                                Linkrsl =  "AND",
                                Item = "tea21",
                                DscValue = "10000",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Groupline = 1,
                                Linkrsl =  "OR",
                                Item = "tea22",
                                DscValue = "10000",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Groupline = 2,
                                Linkrsl =  "AND",
                                Item = "tea23",
                                DscValue = "10000",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 4,
                                Groupline = 2,
                                Linkrsl =  "AND",
                                Item = "tea24",
                                DscValue = "10000",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 5,
                                Groupline = 2,
                                Linkrsl =  "",
                                Item = "tea25",
                                DscValue = "10000",
                                MaxValue = "",
                                ActiveFlag = true
                            }
                        }
                    },
                    //Promo Amount Class Item All Item
                    new PromoRule
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Promo Buy Milk",
                        Cls = 1,
                        Lvl = 2,
                        ItemType = "ALL",
                        ResultType = "V1",
                        PromoActionType = "AMOUNT",
                        PromoActionValue = "1000",
                        MaxValue = "",
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddYears(1),
                        ActiveFlag = true,

                        PromoRuleVariable = new List<PromoRuleVariable>()
                        {
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Code = "itmDisc",
                                Name = "Item Discount",
                                ParamsExpression = "new string[]{\"milk1\",\"milk2\",\"milk3\",\"milk4\",\"milk5\"}",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleExpression = new List<PromoRuleExpression>()
                        {
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 0,
                                Code = "cekItem",
                                Name = "Cek Item Avail",
                                ParamsExpression = "paramsPromo.ItemProduct.Any(q => itmDisc.Contains(q.skuGroup))",
                                Linkexp = "",
                                ActiveFlag = true
                            }
                        }
                    },
                    //Promo Amount Class Item All Item With Requirment Combination AND OR
                    new PromoRule
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Promo Buy Milk Combination",
                        Cls = 1,
                        Lvl = 2,
                        ItemType = "ALL",
                        ResultType = "V1",
                        PromoActionType = "PERCENT",
                        PromoActionValue = "15%",
                        MaxValue = "",
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddYears(1),
                        ActiveFlag = true,

                        PromoRuleVariable = new List<PromoRuleVariable>()
                        {
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Code = "itmDisc01",
                                Name = "Item Discount",
                                ParamsExpression = "new string[]{\"milk6\",\"milk7\",\"milk8\",\"milk9\",\"milk10\"}",
                                ActiveFlag = true
                            },
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Code = "itmDisc02",
                                Name = "Item Discount",
                                ParamsExpression = "new string[]{\"milkcombi6\",\"milkcombi7\",\"milkcombi8\",\"milkcombi9\",\"milkcombi10\"}",
                                ActiveFlag = true
                            },
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Code = "zoneDisc",
                                Name = "Zone Discount",
                                ParamsExpression = "new string[]{\"A\",\"B\",\"C\",\"D\",\"E\"}",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleExpression = new List<PromoRuleExpression>()
                        {
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 1,
                                Code = "cekItem01",
                                Name = "Cek Item Avail",
                                ParamsExpression = "paramsPromo.ItemProduct.Any(q => itmDisc01.Contains(q.skuGroup))",
                                Linkexp = "AND",
                                ActiveFlag = true
                            },
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Groupline = 1,
                                Code = "cekZone01",
                                Name = "Cek Zone Avail",
                                ParamsExpression = "zoneDisc.Contains(paramsPromo.Zone)",
                                Linkexp = "OR",
                                ActiveFlag = true
                            },
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Groupline = 2,
                                Code = "cekItem02",
                                Name = "Cek Item Avail",
                                ParamsExpression = "paramsPromo.ItemProduct.Any(q => itmDisc02.Contains(q.skuGroup))",
                                Linkexp = "AND",
                                ActiveFlag = true
                            },
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 4,
                                Groupline = 2,
                                Code = "cekZone02",
                                Name = "Cek Zone Avail",
                                ParamsExpression = "zoneDisc.Contains(paramsPromo.Zone)",
                                Linkexp = "",
                                ActiveFlag = true
                            }
                        },
                    },
                    //Promo Amount Class Item Custom Item Result AND
                    new PromoRule
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Promo Buy Milk AND",
                        Cls = 1,
                        Lvl = 2,
                        ItemType = "CUSTOM",
                        ResultType = "V1",
                        PromoActionType = "AMOUNT",
                        PromoActionValue = "",
                        MaxValue = "",
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddYears(1),
                        ActiveFlag = true,

                        PromoRuleVariable = new List<PromoRuleVariable>()
                        {
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Code = "itmDisc",
                                Name = "Item Discount",
                                ParamsExpression = "new string[]{\"milk16\",\"milk17\",\"milk18\",\"milk19\",\"milk20\"}",
                                ActiveFlag = true
                            },
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Code = "zoneDisc",
                                Name = "Zone Discount",
                                ParamsExpression = "new string[]{\"F\",\"G\",\"H\"}",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleExpression = new List<PromoRuleExpression>()
                        {
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 0,
                                Code = "cekItem",
                                Name = "Cek Item Avail",
                                ParamsExpression = "paramsPromo.ItemProduct.Any(q => itmDisc.Contains(q.skuGroup))",
                                Linkexp = "AND",
                                ActiveFlag = true
                            },
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Groupline = 0,
                                Code = "cekZone",
                                Name = "Cek Zone Avail",
                                ParamsExpression = "zoneDisc.Contains(paramsPromo.Zone)",
                                Linkexp = "",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleResult = new List<PromoRuleResult>()
                        {
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 0,
                                Linkrsl =  "AND",
                                Item = "milk16",
                                DscValue = "1000",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Groupline = 0,
                                Linkrsl =  "AND",
                                Item = "milk17",
                                DscValue = "2000",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Groupline = 0,
                                Linkrsl =  "AND",
                                Item = "milk18",
                                DscValue = "3000",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 4,
                                Groupline = 0,
                                Linkrsl =  "AND",
                                Item = "milk19",
                                DscValue = "4000",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 5,
                                Groupline = 0,
                                Linkrsl =  "",
                                Item = "milk20",
                                DscValue = "5000",
                                MaxValue = "",
                                ActiveFlag = true
                            }
                        }
                    },
                    //Promo Amount Class Item Custom Item Result OR
                    new PromoRule
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Promo Buy Milk OR",
                        Cls = 1,
                        Lvl = 2,
                        ItemType = "CUSTOM",
                        ResultType = "V1",
                        PromoActionType = "AMOUNT",
                        PromoActionValue = "",
                        MaxValue = "",
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddYears(1),
                        ActiveFlag = true,

                        PromoRuleVariable = new List<PromoRuleVariable>()
                        {
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Code = "itmDisc",
                                Name = "Item Discount",
                                ParamsExpression = "new string[]{\"milk6\",\"milk7\",\"milk8\",\"milk9\",\"milk10\"}",
                                ActiveFlag = true
                            },
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Code = "zoneDisc",
                                Name = "Zone Discount",
                                ParamsExpression = "new string[]{\"F\",\"G\",\"H\"}",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleExpression = new List<PromoRuleExpression>()
                        {
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 0,
                                Code = "cekItem",
                                Name = "Cek Item Avail",
                                ParamsExpression = "paramsPromo.ItemProduct.Any(q => itmDisc.Contains(q.skuGroup))",
                                Linkexp = "AND",
                                ActiveFlag = true
                            },
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Groupline = 0,
                                Code = "cekZone",
                                Name = "Cek Zone Avail",
                                ParamsExpression = "zoneDisc.Contains(paramsPromo.Zone)",
                                Linkexp = "",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleResult = new List<PromoRuleResult>()
                        {
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 1,
                                Linkrsl =  "OR",
                                Item = "milk6",
                                DscValue = "2000",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Groupline = 2,
                                Linkrsl =  "OR",
                                Item = "milk7",
                                DscValue = "3000",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Groupline = 3,
                                Linkrsl =  "OR",
                                Item = "milk8",
                                DscValue = "4000",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 4,
                                Groupline = 4,
                                Linkrsl =  "OR",
                                Item = "milk9",
                                DscValue = "5000",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 5,
                                Groupline = 5,
                                Linkrsl =  "",
                                Item = "milk10",
                                DscValue = "6000",
                                MaxValue = "",
                                ActiveFlag = true
                            }
                        }
                    },
                    //Promo Amount Class Item Custom Item Result Combination AND OR
                    new PromoRule
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Promo Buy Milk Combi Result",
                        Cls = 1,
                        Lvl = 2,
                        ItemType = "CUSTOM",
                        ResultType = "V1",
                        PromoActionType = "AMOUNT",
                        PromoActionValue = "",
                        MaxValue = "",
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddYears(1),
                        ActiveFlag = true,

                        PromoRuleVariable = new List<PromoRuleVariable>()
                        {
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Code = "itmDisc",
                                Name = "Item Discount",
                                ParamsExpression = "new string[]{\"milk11\",\"milk12\",\"milk13\",\"milk14\",\"milk15\"}",
                                ActiveFlag = true
                            },
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Code = "zoneDisc",
                                Name = "Zone Discount",
                                ParamsExpression = "new string[]{\"F\",\"G\",\"H\"}",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleExpression = new List<PromoRuleExpression>()
                        {
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 0,
                                Code = "cekItem",
                                Name = "Cek Item Avail",
                                ParamsExpression = "paramsPromo.ItemProduct.Any(q => itmDisc.Contains(q.skuGroup))",
                                Linkexp = "AND",
                                ActiveFlag = true
                            },
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Groupline = 0,
                                Code = "cekZone",
                                Name = "Cek Zone Avail",
                                ParamsExpression = "zoneDisc.Contains(paramsPromo.Zone)",
                                Linkexp = "",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleResult = new List<PromoRuleResult>()
                        {
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 1,
                                Linkrsl =  "AND",
                                Item = "milk11",
                                DscValue = "3000",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Groupline = 1,
                                Linkrsl =  "OR",
                                Item = "milk12",
                                DscValue = "4000",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Groupline = 2,
                                Linkrsl =  "AND",
                                Item = "milk13",
                                DscValue = "3000",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 4,
                                Groupline = 2,
                                Linkrsl =  "AND",
                                Item = "milk14",
                                DscValue = "4000",
                                MaxValue = "",
                                ActiveFlag = true
                            }
                            ,
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 5,
                                Groupline = 2,
                                Linkrsl =  "",
                                Item = "milk15",
                                DscValue = "3000",
                                MaxValue = "",
                                ActiveFlag = true
                            }
                        }
                    },
                    //Promo Percent Class Cart All Item
                    new PromoRule
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Promo Lunar",
                        Cls = 2,
                        Lvl = 2,
                        ItemType = "ALL",
                        ResultType = "V1",
                        PromoActionType = "PERCENT",
                        PromoActionValue = "10%",
                        MaxValue = "",
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddYears(1),
                        ActiveFlag = true,

                        PromoRuleVariable = new List<PromoRuleVariable>()
                        {
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Code = "startDate",
                                Name = "Start Date",
                                ParamsExpression = "Convert.ToDateTime(\"15-01-2023\")",
                                ActiveFlag = true
                            },
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Code = "endDate",
                                Name = "End Date",
                                ParamsExpression = "Convert.ToDateTime(\"30-01-2023\")",
                                ActiveFlag = true
                            },
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Code = "prmDate",
                                Name = "Params Date",
                                ParamsExpression = "Convert.ToDateTime(paramsPromo.TransDate)",
                                ActiveFlag = true
                            },
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 4,
                                Code = "zoneDisc",
                                Name = "Zone Discount",
                                ParamsExpression = "new string[]{\"A\",\"B\",\"C\",\"D\",\"E\",\"F\",\"G\",\"H\",\"I\"}",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleExpression = new List<PromoRuleExpression>()
                        {
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 0,
                                Code = "cekDateStart",
                                Name = "Cek Date Start",
                                ParamsExpression = "prmDate >= startDate",
                                Linkexp = "AND",
                                ActiveFlag = true
                            },
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Groupline = 0,
                                Code = "cekDateEnd",
                                Name = "Cek Date End",
                                ParamsExpression = "endDate >= prmDate",
                                Linkexp = "AND",
                                ActiveFlag = true
                            },
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Groupline = 0,
                                Code = "cekTotalPrice",
                                Name = "Cek Total Price",
                                ParamsExpression = "total_price >= 100000",
                                Linkexp = "AND",
                                ActiveFlag = true
                            },
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 4,
                                Groupline = 0,
                                Code = "cekZone",
                                Name = "Cek Zone Avail",
                                ParamsExpression = "!zoneDisc.Contains(paramsPromo.Zone)",
                                Linkexp = "",
                                ActiveFlag = true
                            }
                        }
                    },
                    //Promo Percent Class Cart Custom Item Result AND
                    new PromoRule
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Promo Lunar AND",
                        Cls = 2,
                        Lvl = 2,
                        ItemType = "CUSTOM",
                        ResultType = "V1",
                        PromoActionType = "PERCENT",
                        PromoActionValue = "10%",
                        MaxValue = "",
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddYears(1),
                        ActiveFlag = true,

                        PromoRuleVariable = new List<PromoRuleVariable>()
                        {
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Code = "startDate",
                                Name = "Start Date",
                                ParamsExpression = "Convert.ToDateTime(\"15-01-2023\")",
                                ActiveFlag = true
                            },
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Code = "endDate",
                                Name = "End Date",
                                ParamsExpression = "Convert.ToDateTime(\"30-01-2023\")",
                                ActiveFlag = true
                            },
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Code = "prmDate",
                                Name = "Params Date",
                                ParamsExpression = "Convert.ToDateTime(paramsPromo.TransDate)",
                                ActiveFlag = true
                            },
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 4,
                                Code = "zoneDisc",
                                Name = "Zone Discount",
                                ParamsExpression = "new string[]{\"A\",\"B\",\"C\"}",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleExpression = new List<PromoRuleExpression>()
                        {
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 0,
                                Code = "cekDateStart",
                                Name = "Cek Date Start",
                                ParamsExpression = "prmDate >= startDate",
                                Linkexp = "AND",
                                ActiveFlag = true
                            },
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Groupline = 0,
                                Code = "cekDateEnd",
                                Name = "Cek Date End",
                                ParamsExpression = "endDate >= prmDate",
                                Linkexp = "AND",
                                ActiveFlag = true
                            },
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Groupline = 0,
                                Code = "cekZone",
                                Name = "Cek Zone Avail",
                                ParamsExpression = "zoneDisc.Contains(paramsPromo.Zone)",
                                Linkexp = "",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleResult = new List<PromoRuleResult>()
                        {
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 0,
                                Linkrsl =  "AND",
                                Item = "milk21",
                                DscValue = "10%",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Groupline = 0,
                                Linkrsl =  "AND",
                                Item = "milk22",
                                DscValue = "10%",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Groupline = 0,
                                Linkrsl =  "AND",
                                Item = "milk23",
                                DscValue = "10%",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 4,
                                Groupline = 0,
                                Linkrsl =  "AND",
                                Item = "milk24",
                                DscValue = "10%",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 5,
                                Groupline = 0,
                                Linkrsl =  "",
                                Item = "milk25",
                                DscValue = "10%",
                                MaxValue = "",
                                ActiveFlag = true
                            }
                        }
                    },
                    //Promo Percent Class Cart Custom Item Result OR
                    new PromoRule
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Promo Lunar OR",
                        Cls = 2,
                        Lvl = 2,
                        ItemType = "CUSTOM",
                        ResultType = "V1",
                        PromoActionType = "PERCENT",
                        PromoActionValue = "15%",
                        MaxValue = "",
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddYears(1),
                        ActiveFlag = true,

                        PromoRuleVariable = new List<PromoRuleVariable>()
                        {
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Code = "startDate",
                                Name = "Start Date",
                                ParamsExpression = "Convert.ToDateTime(\"15-01-2023\")",
                                ActiveFlag = true
                            },
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Code = "endDate",
                                Name = "End Date",
                                ParamsExpression = "Convert.ToDateTime(\"30-01-2023\")",
                                ActiveFlag = true
                            },
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Code = "prmDate",
                                Name = "Params Date",
                                ParamsExpression = "Convert.ToDateTime(paramsPromo.TransDate)",
                                ActiveFlag = true
                            },
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 4,
                                Code = "zoneDisc",
                                Name = "Zone Discount",
                                ParamsExpression = "new string[]{\"D\",\"E\",\"F\"}",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleExpression = new List<PromoRuleExpression>()
                        {
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 0,
                                Code = "cekDateStart",
                                Name = "Cek Date Start",
                                ParamsExpression = "prmDate >= startDate",
                                Linkexp = "AND",
                                ActiveFlag = true
                            },
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Groupline = 0,
                                Code = "cekDateEnd",
                                Name = "Cek Date End",
                                ParamsExpression = "endDate >= prmDate",
                                Linkexp = "AND",
                                ActiveFlag = true
                            },
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Groupline = 0,
                                Code = "cekZone",
                                Name = "Cek Zone Avail",
                                ParamsExpression = "zoneDisc.Contains(paramsPromo.Zone)",
                                Linkexp = "",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleResult = new List<PromoRuleResult>()
                        {
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 1,
                                Linkrsl =  "OR",
                                Item = "milk21",
                                DscValue = "15%",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Groupline = 2,
                                Linkrsl =  "OR",
                                Item = "milk22",
                                DscValue = "15%",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Groupline = 3,
                                Linkrsl =  "OR",
                                Item = "milk23",
                                DscValue = "15%",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 4,
                                Groupline = 4,
                                Linkrsl =  "OR",
                                Item = "milk24",
                                DscValue = "15%",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 5,
                                Groupline = 5,
                                Linkrsl =  "",
                                Item = "milk25",
                                DscValue = "15%",
                                MaxValue = "",
                                ActiveFlag = true
                            }
                        }
                    },
                    //Promo Percent Class Cart Custom Item Result Combination AND OR
                    new PromoRule
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Promo Lunar COMBI",
                        Cls = 2,
                        Lvl = 2,
                        ItemType = "CUSTOM",
                        ResultType = "V1",
                        PromoActionType = "PERCENT",
                        PromoActionValue = "20%",
                        MaxValue = "",
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddYears(1),
                        ActiveFlag = true,

                        PromoRuleVariable = new List<PromoRuleVariable>()
                        {
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Code = "startDate",
                                Name = "Start Date",
                                ParamsExpression = "Convert.ToDateTime(\"15-01-2023\")",
                                ActiveFlag = true
                            },
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Code = "endDate",
                                Name = "End Date",
                                ParamsExpression = "Convert.ToDateTime(\"15-01-2023\")",
                                ActiveFlag = true
                            },
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Code = "prmDate",
                                Name = "Params Date",
                                ParamsExpression = "Convert.ToDateTime(paramsPromo.TransDate)",
                                ActiveFlag = true
                            },
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 4,
                                Code = "zoneDisc",
                                Name = "Zone Discount",
                                ParamsExpression = "new string[]{\"G\",\"H\",\"I\"}",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleExpression = new List<PromoRuleExpression>()
                        {
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 0,
                                Code = "cekDateStart",
                                Name = "Cek Date Start",
                                ParamsExpression = "prmDate >= startDate",
                                Linkexp = "AND",
                                ActiveFlag = true
                            },
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Groupline = 0,
                                Code = "cekDateEnd",
                                Name = "Cek Date End",
                                ParamsExpression = "endDate >= prmDate",
                                Linkexp = "AND",
                                ActiveFlag = true
                            },
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Groupline = 0,
                                Code = "cekZone",
                                Name = "Cek Zone Avail",
                                ParamsExpression = "zoneDisc.Contains(paramsPromo.Zone)",
                                Linkexp = "",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleResult = new List<PromoRuleResult>()
                        {
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 1,
                                Linkrsl =  "AND",
                                Item = "milk21",
                                DscValue = "20%",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Groupline = 1,
                                Linkrsl =  "OR",
                                Item = "milk22",
                                DscValue = "20%",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Groupline = 2,
                                Linkrsl =  "AND",
                                Item = "milk23",
                                DscValue = "20%",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 4,
                                Groupline = 2,
                                Linkrsl =  "AND",
                                Item = "milk24",
                                DscValue = "20%",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 5,
                                Groupline = 2,
                                Linkrsl =  "",
                                Item = "milk25",
                                DscValue = "20%",
                                MaxValue = "",
                                ActiveFlag = true
                            }
                        }
                    },
                    //Promo Percent Class Item All Item
                    new PromoRule
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Promo Buy Cho-Cho",
                        Cls = 1,
                        Lvl = 2,
                        ItemType = "ALL",
                        ResultType = "V1",
                        PromoActionType = "PERCENT",
                        PromoActionValue = "5%",
                        MaxValue = "",
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddYears(1),
                        ActiveFlag = true,

                        PromoRuleVariable = new List<PromoRuleVariable>()
                        {
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Code = "itmDisc",
                                Name = "Item Discount",
                                ParamsExpression = "new string[]{\"cho1\",\"cho2\",\"cho3\",\"cho4\",\"cho5\"}",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleExpression = new List<PromoRuleExpression>()
                        {
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 0,
                                Code = "cekItem",
                                Name = "Cek Item Avail",
                                ParamsExpression = "(paramsPromo.ItemProduct.Where(q => itmDisc.Contains(q.skuGroup))).Sum(q => q.qty) >= 10",
                                Linkexp = "",
                                ActiveFlag = true
                            }
                        }
                    },
                    //Promo Percent Class Item Custom Item Result AND
                    new PromoRule
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Promo Buy Tea AND",
                        Cls = 1,
                        Lvl = 2,
                        ItemType = "CUSTOM",
                        ResultType = "V1",
                        PromoActionType = "PERCENT",
                        PromoActionValue = "",
                        MaxValue = "",
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddYears(1),
                        ActiveFlag = true,

                        PromoRuleVariable = new List<PromoRuleVariable>()
                        {
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Code = "itmDisc",
                                Name = "Item Discount",
                                ParamsExpression = "new string[]{\"tea11\",\"tea12\",\"tea13\",\"tea14\",\"tea15\"}",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleExpression = new List<PromoRuleExpression>()
                        {
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 0,
                                Code = "cekItem",
                                Name = "Cek Item Avail",
                                ParamsExpression = "paramsPromo.ItemProduct.Any(q => itmDisc.Contains(q.skuGroup))",
                                Linkexp = "",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleResult = new List<PromoRuleResult>()
                        {
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 0,
                                Linkrsl =  "AND",
                                Item = "tea11",
                                DscValue = "5%",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Groupline = 0,
                                Linkrsl =  "AND",
                                Item = "tea12",
                                DscValue = "5%",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Groupline = 0,
                                Linkrsl =  "AND",
                                Item = "tea13",
                                DscValue = "10%",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 4,
                                Groupline = 0,
                                Linkrsl =  "AND",
                                Item = "tea14",
                                DscValue = "10%",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 5,
                                Groupline = 0,
                                Linkrsl =  "",
                                Item = "tea15",
                                DscValue = "10%",
                                MaxValue = "",
                                ActiveFlag = true
                            }
                        }
                    },
                    //Promo Percent Class Item Custom Item Result OR
                    new PromoRule
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Promo Buy Tea OR",
                        Cls = 1,
                        Lvl = 2,
                        ItemType = "CUSTOM",
                        ResultType = "V1",
                        PromoActionType = "PERCENT",
                        PromoActionValue = "",
                        MaxValue = "",
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddYears(1),
                        ActiveFlag = true,

                        PromoRuleVariable = new List<PromoRuleVariable>()
                        {
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Code = "itmDisc",
                                Name = "Item Discount",
                                ParamsExpression = "new string[]{\"tea6\",\"tea7\",\"tea8\",\"tea9\",\"tea10\"}",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleExpression = new List<PromoRuleExpression>()
                        {
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 0,
                                Code = "cekItem",
                                Name = "Cek Item Avail",
                                ParamsExpression = "paramsPromo.ItemProduct.Any(q => itmDisc.Contains(q.skuGroup))",
                                Linkexp = "",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleResult = new List<PromoRuleResult>()
                        {
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 1,
                                Linkrsl =  "OR",
                                Item = "tea6",
                                DscValue = "5%",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Groupline = 2,
                                Linkrsl =  "OR",
                                Item = "tea7",
                                DscValue = "10%",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Groupline = 3,
                                Linkrsl =  "OR",
                                Item = "tea8",
                                DscValue = "15%",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 4,
                                Groupline = 4,
                                Linkrsl =  "OR",
                                Item = "tea9",
                                DscValue = "20%",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 5,
                                Groupline = 5,
                                Linkrsl =  "",
                                Item = "tea10",
                                DscValue = "25%",
                                MaxValue = "",
                                ActiveFlag = true
                            }
                        }
                    },
                    //Promo Percent Class Item Custom Item Result OR ZONE Z
                    new PromoRule
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Promo Buy Tea OR",
                        Cls = 1,
                        Lvl = 2,
                        ItemType = "CUSTOM",
                        ResultType = "V1",
                        PromoActionType = "PERCENT",
                        PromoActionValue = "",
                        MaxValue = "",
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddYears(1),
                        ActiveFlag = true,

                        PromoRuleVariable = new List<PromoRuleVariable>()
                        {
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Code = "itmDisc",
                                Name = "Item Discount",
                                ParamsExpression = "new string[]{\"tea6\",\"tea7\",\"tea8\",\"tea9\",\"tea10\"}",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleExpression = new List<PromoRuleExpression>()
                        {
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 0,
                                Code = "cekItem",
                                Name = "Cek Item Avail",
                                ParamsExpression = "paramsPromo.ItemProduct.Any(q => itmDisc.Contains(q.skuGroup))",
                                Linkexp = "AND",
                                ActiveFlag = true
                            },
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Groupline = 0,
                                Code = "cekZone",
                                Name = "Cek Zone Avail",
                                ParamsExpression = "paramsPromo.Zone == \"Z\"",
                                Linkexp = "",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleResult = new List<PromoRuleResult>()
                        {
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 1,
                                Linkrsl =  "OR",
                                Item = "tea6",
                                DscValue = "10%",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Groupline = 2,
                                Linkrsl =  "OR",
                                Item = "tea7",
                                DscValue = "15%",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Groupline = 3,
                                Linkrsl =  "OR",
                                Item = "tea8",
                                DscValue = "20%",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 4,
                                Groupline = 4,
                                Linkrsl =  "OR",
                                Item = "tea9",
                                DscValue = "25%",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 5,
                                Groupline = 5,
                                Linkrsl =  "",
                                Item = "tea10",
                                DscValue = "30%",
                                MaxValue = "",
                                ActiveFlag = true
                            }
                        }
                    },
                    //Promo Percent Class Item Custom Item Result Combination AND OR
                    new PromoRule
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Promo Buy Tea Combi",
                        Cls = 1,
                        Lvl = 2,
                        ItemType = "CUSTOM",
                        ResultType = "V1",
                        PromoActionType = "PERCENT",
                        PromoActionValue = "",
                        MaxValue = "",
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddYears(1),
                        ActiveFlag = true,

                        PromoRuleVariable = new List<PromoRuleVariable>()
                        {
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Code = "itmDisc",
                                Name = "Item Discount",
                                ParamsExpression = "new string[]{\"tea1\",\"tea2\",\"tea3\",\"tea4\",\"tea5\"}",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleExpression = new List<PromoRuleExpression>()
                        {
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 0,
                                Code = "cekItem",
                                Name = "Cek Item Avail",
                                ParamsExpression = "paramsPromo.ItemProduct.Any(q => itmDisc.Contains(q.skuGroup))",
                                Linkexp = "",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleResult = new List<PromoRuleResult>()
                        {
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 1,
                                Linkrsl =  "AND",
                                Item = "tea1",
                                DscValue = "5%",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Groupline = 1,
                                Linkrsl =  "OR",
                                Item = "tea2",
                                DscValue = "5%",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Groupline = 2,
                                Linkrsl =  "AND",
                                Item = "tea3",
                                DscValue = "10%",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 4,
                                Groupline = 2,
                                Linkrsl =  "AND",
                                Item = "tea4",
                                DscValue = "10%",
                                MaxValue = "",
                                ActiveFlag = true
                            }
                            ,
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 5,
                                Groupline = 2,
                                Linkrsl =  "",
                                Item = "tea5",
                                DscValue = "10%",
                                MaxValue = "",
                                ActiveFlag = true
                            }
                        }
                    },
                    //Promo Item Class Item Custom Item Result AND
                    new PromoRule
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Promo Buy Coffe AND",
                        Cls = 1,
                        Lvl = 2,
                        ItemType = "CUSTOM",
                        ResultType = "V1",
                        PromoActionType = "ITEM",
                        PromoActionValue = "",
                        MaxValue = "",
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddYears(1),
                        ActiveFlag = true,

                        PromoRuleVariable = new List<PromoRuleVariable>()
                        {
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Code = "itmDisc",
                                Name = "Item Discount",
                                ParamsExpression = "new string[]{\"coffe1\",\"coffe2\",\"coffe3\",\"coffe4\",\"coffe5\"}",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleExpression = new List<PromoRuleExpression>()
                        {
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 0,
                                Code = "cekItem",
                                Name = "Cek Item Avail",
                                ParamsExpression = "paramsPromo.ItemProduct.Any(q => itmDisc.Contains(q.skuGroup))",
                                Linkexp = "",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleResult = new List<PromoRuleResult>()
                        {
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 0,
                                Linkrsl =  "AND",
                                Item = "coffe1",
                                DscValue = "FREE",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Groupline = 0,
                                Linkrsl =  "AND",
                                Item = "coffe2",
                                DscValue = "FREE",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Groupline = 0,
                                Linkrsl =  "AND",
                                Item = "coffe3",
                                DscValue = "FREE",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 4,
                                Groupline = 0,
                                Linkrsl =  "AND",
                                Item = "coffe4",
                                DscValue = "FREE",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 5,
                                Groupline = 0,
                                Linkrsl =  "",
                                Item = "coffe5",
                                DscValue = "FREE",
                                MaxValue = "",
                                ActiveFlag = true
                            }
                        }
                    },
                    //Promo Item Class Item Custom Item Result OR
                    new PromoRule
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Promo Buy Coffe OR",
                        Cls = 1,
                        Lvl = 2,
                        ItemType = "CUSTOM",
                        ResultType = "V1",
                        PromoActionType = "ITEM",
                        PromoActionValue = "",
                        MaxValue = "",
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddYears(1),
                        ActiveFlag = true,

                        PromoRuleVariable = new List<PromoRuleVariable>()
                        {
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Code = "itmDisc",
                                Name = "Item Discount",
                                ParamsExpression = "new string[]{\"coffe6\",\"coffe7\",\"coffe8\",\"coffe9\",\"coffe10\"}",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleExpression = new List<PromoRuleExpression>()
                        {
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 0,
                                Code = "cekItem",
                                Name = "Cek Item Avail",
                                ParamsExpression = "paramsPromo.ItemProduct.Any(q => itmDisc.Contains(q.skuGroup))",
                                Linkexp = "",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleResult = new List<PromoRuleResult>()
                        {
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 1,
                                Linkrsl =  "OR",
                                Item = "coffe6",
                                DscValue = "FREE",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Groupline = 2,
                                Linkrsl =  "OR",
                                Item = "coffe7",
                                DscValue = "FREE",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Groupline = 3,
                                Linkrsl =  "OR",
                                Item = "coffe8",
                                DscValue = "FREE",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 4,
                                Groupline = 4,
                                Linkrsl =  "OR",
                                Item = "coffe9",
                                DscValue = "FREE",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 5,
                                Groupline = 5,
                                Linkrsl =  "",
                                Item = "coffe10",
                                DscValue = "FREE",
                                MaxValue = "",
                                ActiveFlag = true
                            }
                        }
                    },
                    //Promo Item Class Item Custom Item Result OR ZONE A
                    new PromoRule
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Promo Buy Coffe OR",
                        Cls = 1,
                        Lvl = 2,
                        ItemType = "CUSTOM",
                        ResultType = "V1",
                        PromoActionType = "ITEM",
                        PromoActionValue = "",
                        MaxValue = "",
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddYears(1),
                        ActiveFlag = true,

                        PromoRuleVariable = new List<PromoRuleVariable>()
                        {
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Code = "itmDisc",
                                Name = "Item Discount",
                                ParamsExpression = "new string[]{\"coffe6\",\"coffe7\",\"coffe8\",\"coffe9\",\"coffe10\"}",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleExpression = new List<PromoRuleExpression>()
                        {
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 0,
                                Code = "cekItem",
                                Name = "Cek Item Avail",
                                ParamsExpression = "paramsPromo.ItemProduct.Any(q => itmDisc.Contains(q.skuGroup))",
                                Linkexp = "AND",
                                ActiveFlag = true
                            },
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Groupline = 0,
                                Code = "cekZone",
                                Name = "Cek Item Avail",
                                ParamsExpression = "paramsPromo.Zone == \"A\"",
                                Linkexp = "",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleResult = new List<PromoRuleResult>()
                        {
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 1,
                                Linkrsl =  "OR",
                                Item = "coffe6",
                                DscValue = "FREE",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Groupline = 2,
                                Linkrsl =  "OR",
                                Item = "coffe7",
                                DscValue = "FREE",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Groupline = 3,
                                Linkrsl =  "OR",
                                Item = "coffe8",
                                DscValue = "FREE",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 4,
                                Groupline = 4,
                                Linkrsl =  "OR",
                                Item = "coffe9",
                                DscValue = "FREE",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 5,
                                Groupline = 5,
                                Linkrsl =  "",
                                Item = "coffe10",
                                DscValue = "FREE",
                                MaxValue = "",
                                ActiveFlag = true
                            }
                        }
                    },
                    //Promo Item Class Item Custom Item Result Combination AND OR
                    new PromoRule
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Promo Buy Coffe Combi",
                        Cls = 1,
                        Lvl = 2,
                        ItemType = "CUSTOM",
                        ResultType = "V1",
                        PromoActionType = "ITEM",
                        PromoActionValue = "",
                        MaxValue = "",
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddYears(1),
                        ActiveFlag = true,

                        PromoRuleVariable = new List<PromoRuleVariable>()
                        {
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Code = "itmDisc",
                                Name = "Item Discount",
                                ParamsExpression = "new string[]{\"coffe11\",\"coffe12\",\"coffe13\",\"coffe14\",\"coffe15\"}",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleExpression = new List<PromoRuleExpression>()
                        {
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 0,
                                Code = "cekItem",
                                Name = "Cek Item Avail",
                                ParamsExpression = "paramsPromo.ItemProduct.Any(q => itmDisc.Contains(q.skuGroup))",
                                Linkexp = "",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleResult = new List<PromoRuleResult>()
                        {
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 1,
                                Linkrsl =  "AND",
                                Item = "coffe11",
                                DscValue = "FREE",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Groupline = 1,
                                Linkrsl =  "OR",
                                Item = "coffe12",
                                DscValue = "FREE",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Groupline = 2,
                                Linkrsl =  "AND",
                                Item = "coffe13",
                                DscValue = "FREE",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 4,
                                Groupline = 2,
                                Linkrsl =  "AND",
                                Item = "coffe14",
                                DscValue = "FREE",
                                MaxValue = "",
                                ActiveFlag = true
                            }
                            ,
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 5,
                                Groupline = 2,
                                Linkrsl =  "",
                                Item = "coffe15",
                                DscValue = "FREE",
                                MaxValue = "",
                                ActiveFlag = true
                            }
                        }
                    },
                    //Promo Item Class Item Custom Item Result Combination AND OR TYPE 2
                    new PromoRule
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Promo Buy Coffe Combi",
                        Cls = 1,
                        Lvl = 2,
                        ItemType = "CUSTOM",
                        ResultType = "V1",
                        PromoActionType = "ITEM",
                        PromoActionValue = "",
                        MaxValue = "",
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddYears(1),
                        ActiveFlag = true,

                        PromoRuleVariable = new List<PromoRuleVariable>()
                        {
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Code = "itmDisc",
                                Name = "Item Discount",
                                ParamsExpression = "new string[]{\"coffe11\",\"coffe12\",\"coffe13\",\"coffe14\",\"coffe15\"}",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleExpression = new List<PromoRuleExpression>()
                        {
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 0,
                                Code = "cekItem",
                                Name = "Cek Item Avail",
                                ParamsExpression = "paramsPromo.ItemProduct.Any(q => itmDisc.Contains(q.skuGroup))",
                                Linkexp = "AND",
                                ActiveFlag = true
                            },
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Groupline = 0,
                                Code = "cekZone",
                                Name = "Cek Item Avail",
                                ParamsExpression = "paramsPromo.Zone == \"A\"",
                                Linkexp = "",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleResult = new List<PromoRuleResult>()
                        {
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 1,
                                Linkrsl =  "AND",
                                Item = "coffe11",
                                DscValue = "FREE",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Groupline = 1,
                                Linkrsl =  "OR",
                                Item = "coffe12",
                                DscValue = "FREE",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Groupline = 2,
                                Linkrsl =  "AND",
                                Item = "coffe13",
                                DscValue = "FREE",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 4,
                                Groupline = 2,
                                Linkrsl =  "AND",
                                Item = "coffe14",
                                DscValue = "FREE",
                                MaxValue = "",
                                ActiveFlag = true
                            }
                            ,
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 5,
                                Groupline = 2,
                                Linkrsl =  "",
                                Item = "coffe15",
                                DscValue = "FREE",
                                MaxValue = "",
                                ActiveFlag = true
                            }
                        }
                    },
                    //Promo SP Class Item Custom Item Result AND
                    new PromoRule
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Promo Buy Coffe AND SP",
                        Cls = 1,
                        Lvl = 2,
                        ItemType = "CUSTOM",
                        ResultType = "V1",
                        PromoActionType = "SP",
                        PromoActionValue = "",
                        MaxValue = "",
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddYears(1),
                        ActiveFlag = true,

                        PromoRuleVariable = new List<PromoRuleVariable>()
                        {
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Code = "itmDisc",
                                Name = "Item Discount",
                                ParamsExpression = "new string[]{\"coffe16\",\"coffe17\",\"coffe18\",\"coffe19\",\"coffe20\"}",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleExpression = new List<PromoRuleExpression>()
                        {
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 0,
                                Code = "cekItem",
                                Name = "Cek Item Avail",
                                ParamsExpression = "paramsPromo.ItemProduct.Any(q => itmDisc.Contains(q.skuGroup))",
                                Linkexp = "",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleResult = new List<PromoRuleResult>()
                        {
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 0,
                                Linkrsl =  "AND",
                                Item = "coffe16",
                                DscValue = "8000",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Groupline = 0,
                                Linkrsl =  "AND",
                                Item = "coffe17",
                                DscValue = "6000",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Groupline = 0,
                                Linkrsl =  "AND",
                                Item = "coffe18",
                                DscValue = "8000",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 4,
                                Groupline = 0,
                                Linkrsl =  "AND",
                                Item = "coffe19",
                                DscValue = "7000",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 5,
                                Groupline = 0,
                                Linkrsl =  "",
                                Item = "coffe20",
                                DscValue = "8000",
                                MaxValue = "",
                                ActiveFlag = true
                            }
                        }
                    },
                    //Promo SP Class Item Custom Item Result OR
                    new PromoRule
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Promo Buy Coffe OR SP",
                        Cls = 1,
                        Lvl = 2,
                        ItemType = "CUSTOM",
                        ResultType = "V1",
                        PromoActionType = "SP",
                        PromoActionValue = "",
                        MaxValue = "",
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddYears(1),
                        ActiveFlag = true,

                        PromoRuleVariable = new List<PromoRuleVariable>()
                        {
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Code = "itmDisc",
                                Name = "Item Discount",
                                ParamsExpression = "new string[]{\"coffe21\",\"coffe22\",\"coffe23\",\"coffe24\",\"coffe25\"}",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleExpression = new List<PromoRuleExpression>()
                        {
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 0,
                                Code = "cekItem",
                                Name = "Cek Item Avail",
                                ParamsExpression = "paramsPromo.ItemProduct.Any(q => itmDisc.Contains(q.skuGroup))",
                                Linkexp = "",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleResult = new List<PromoRuleResult>()
                        {
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 1,
                                Linkrsl =  "OR",
                                Item = "coffe21",
                                DscValue = "9000",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Groupline = 2,
                                Linkrsl =  "OR",
                                Item = "coffe22",
                                DscValue = "7000",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Groupline = 3,
                                Linkrsl =  "OR",
                                Item = "coffe23",
                                DscValue = "9500",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 4,
                                Groupline = 4,
                                Linkrsl =  "OR",
                                Item = "coffe24",
                                DscValue = "7500",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 5,
                                Groupline = 5,
                                Linkrsl =  "",
                                Item = "coffe25",
                                DscValue = "6000",
                                MaxValue = "",
                                ActiveFlag = true
                            }
                        }
                    },
                    //Promo SP Class Item Custom Item Result Combination AND OR
                    new PromoRule
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Promo Buy Coffe Combi SP",
                        Cls = 1,
                        Lvl = 2,
                        ItemType = "CUSTOM",
                        ResultType = "V1",
                        PromoActionType = "SP",
                        PromoActionValue = "",
                        MaxValue = "",
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddYears(1),
                        ActiveFlag = true,

                        PromoRuleVariable = new List<PromoRuleVariable>()
                        {
                            new PromoRuleVariable
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Code = "itmDisc",
                                Name = "Item Discount",
                                ParamsExpression = "new string[]{\"coffe26\",\"coffe27\",\"coffe28\",\"coffe29\",\"coffe30\"}",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleExpression = new List<PromoRuleExpression>()
                        {
                            new PromoRuleExpression
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 0,
                                Code = "cekItem",
                                Name = "Cek Item Avail",
                                ParamsExpression = "paramsPromo.ItemProduct.Any(q => itmDisc.Contains(q.skuGroup))",
                                Linkexp = "",
                                ActiveFlag = true
                            }
                        },
                        PromoRuleResult = new List<PromoRuleResult>()
                        {
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 1,
                                Groupline = 1,
                                Linkrsl =  "AND",
                                Item = "coffe26",
                                DscValue = "6000",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 2,
                                Groupline = 1,
                                Linkrsl =  "OR",
                                Item = "coffe27",
                                DscValue = "8000",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 3,
                                Groupline = 2,
                                Linkrsl =  "AND",
                                Item = "coffe28",
                                DscValue = "9000",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 4,
                                Groupline = 2,
                                Linkrsl =  "AND",
                                Item = "coffe29",
                                DscValue = "8000",
                                MaxValue = "",
                                ActiveFlag = true
                            },
                            new PromoRuleResult
                            {
                                Id = Guid.NewGuid().ToString(),
                                Linenum = 5,
                                Groupline = 2,
                                Linkrsl =  "",
                                Item = "coffe30",
                                DscValue = "7500",
                                MaxValue = "",
                                ActiveFlag = true
                            }
                        }
                    }
                }
            }
        };
    }
}
