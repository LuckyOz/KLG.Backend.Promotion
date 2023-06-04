
using KLG.Backend.Promotion.Services.Business.Promotion;
using KLG.Backend.Promotion.Services.Entities;
using KLG.Library.Microservice;
using KLG.Library.Microservice.Configuration;
using KLG.Library.Microservice.DataAccess;
using KLG.Library.Microservice.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace KLG.Backend.Promotion.Services.Controllers.RestApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController : KLGApiController<DefaultDbContext>
    {
        private readonly IPromotionManager _promotionManager;

        public PromotionController(
            IPromotionManager promotionManager,
            IKLGDbProvider<DefaultDbContext> dbProvider, IKLGConfiguration configuration,
            IKLGMessagingProvider messageProvider, Serilog.ILogger logger) 
            : base(dbProvider, configuration, messageProvider, logger)
        {
            _promotionManager = promotionManager;
        }


    }
}
