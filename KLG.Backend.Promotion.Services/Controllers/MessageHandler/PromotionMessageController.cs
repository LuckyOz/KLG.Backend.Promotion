
using KLG.Backend.Promotion.Services.Entities;
using KLG.Library.Microservice;
using KLG.Library.Microservice.Configuration;
using KLG.Library.Microservice.DataAccess;
using KLG.Library.Microservice.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace KLG.Backend.Promotion.Services.Controllers.MessageHandler
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionMessageController : KLGApiController<DefaultDbContext>
    {
        public PromotionMessageController(
            IKLGDbProvider<DefaultDbContext> dbProvider, IKLGConfiguration configuration,
            IKLGMessagingProvider messageProvider, Serilog.ILogger logger)
            : base(dbProvider, configuration, messageProvider, logger)
        {
                
        }
    }
}
