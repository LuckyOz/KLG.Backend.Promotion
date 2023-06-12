
using KLG.Backend.Promotion.Services.Controllers.RestApi;
using KLG.Backend.Promotion.Tests.Helper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace KLG.Backend.Promotion.Tests.FindPromo
{
    public class FindPromoTest
    {
        private readonly SetupTesting _setupTesting;

        public FindPromoTest()
        {
            _setupTesting = new SetupTesting();
        }

        [Fact]
        //Test Untuk Mencari Promo Type Amount di Level Cart dan Apply ke Semua Item
        public async Task Find_cart_amount_all()
        {
            //Run Testing and get Result
            PromoController promoController = await _setupTesting.GetPromoController(FindPromoRequest.Find_cart_amount_all);
            var resultFindPromoController = await promoController.FindPromo(FindPromoRequest.Find_cart_amount_all);
            var resultOk = Assert.IsType<OkObjectResult>(resultFindPromoController.Result);

            //Convert Result to String for Checking
            var dataResultFind = JsonConvert.SerializeObject(resultOk.Value);
            var dataResponse = JsonConvert.SerializeObject(FindPromoResponse.Find_cart_amount_all);

            //Cek Testing Success or not with response
            Assert.True(dataResponse.Equals(dataResultFind));
        }
    }
}
