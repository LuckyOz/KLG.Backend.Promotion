
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using KLG.Library.Microservice;
using KLG.Library.Microservice.Messaging;
using KLG.Library.Microservice.DataAccess;
using KLG.Library.Microservice.Configuration;
using KLG.Backend.Promotion.Services.Entities;
using KLG.Backend.Promotion.Services.Business.Promotion;
using KLG.Backend.Promotion.Models.Request;

namespace KLG.Backend.Promotion.Services.Controllers.RestApi
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class PromoController : KLGApiController<DefaultDbContext>
    {
        private readonly IPromoManager _promotionManager;

        public PromoController(
            IPromoManager promotionManager,
            IKLGDbProvider<DefaultDbContext> dbProvider, IKLGConfiguration configuration,
            IKLGMessagingProvider messageProvider, Serilog.ILogger logger) 
            : base(dbProvider, configuration, messageProvider, logger)
        {
            _promotionManager = promotionManager;
        }

        [HttpPost("find_promo")]
        public async Task<ActionResult<List<FindPromoResponseDto>>> FindPromo([FromBody] PromoRequestDto promoRequestDto)
        {
            List<FindPromoResponseDto> response = await _promotionManager.FindPromo(promoRequestDto);

            if (response.Count > 0) {
                return Ok(response);
            }

            return NotFound();
        }

        [HttpGet]
        [Route("insert_data_default_naomi")]
        public async Task<IActionResult> InsertDataDefaultNaomi()
        {
            try {
                await _promotionManager.InsertDataDefault();
                return Ok();
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }
    }
}
