
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using KLG.Library.Microservice;
using KLG.Library.Microservice.Messaging;
using KLG.Library.Microservice.DataAccess;
using KLG.Library.Microservice.Configuration;
using KLG.Backend.Promotion.Models.Request;
using KLG.Backend.Promotion.Services.Entities;
using KLG.Backend.Promotion.Services.Business.Promotion;

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
        public async Task<ActionResult<ServiceResponse<List<FindPromoResponseDto>>>> FindPromo([FromBody]PromoRequestDto promoRequestDto)
        {
            ServiceResponse<List<FindPromoResponseDto>> response = await _promotionManager.FindPromo(promoRequestDto);

            if (response.Data != null && response.Data.Count > 0 && response.Success) {
                return Ok(response);
            }

            return NotFound(response);
        }

        [HttpPost("validate_promo")]
        public async Task<ActionResult<ServiceResponse<ValidatePromoResponseDto>>> ValidatePromo([FromBody]PromoRequestDto promoRequestDto)
        {
            ServiceResponse<ValidatePromoResponseDto> response = await _promotionManager.ValidatePromo(promoRequestDto);

            if (response.Data != null && response.Success) {
                return Ok(response);
            }

            return NotFound(response);
        }

        [HttpPost("calculate_promo")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<ServiceResponse<CalculatePromoResponseDto>>> CalculatePromo([FromBody]PromoRequestDto promoRequestDto)
        {
            ServiceResponse<CalculatePromoResponseDto> response = await _promotionManager.CalculatePromo(promoRequestDto);

            if (response.Data != null && response.Success)
            {
                return Ok(response);
            }

            return NotFound(response);
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
