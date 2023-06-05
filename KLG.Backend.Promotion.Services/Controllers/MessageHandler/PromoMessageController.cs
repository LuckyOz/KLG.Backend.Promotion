
using Microsoft.AspNetCore.Mvc;
using KLG.Library.Microservice;
using KLG.Library.Microservice.Messaging;
using KLG.Library.Microservice.DataAccess;
using KLG.Library.Microservice.Configuration;
using KLG.Backend.Promotion.Services.Entities;

namespace KLG.Backend.Promotion.Services.Controllers.MessageHandler
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromoMessageController : KLGApiController<DefaultDbContext>
    {
        public PromoMessageController(
            IKLGDbProvider<DefaultDbContext> dbProvider, IKLGConfiguration configuration,
            IKLGMessagingProvider messageProvider, Serilog.ILogger logger)
            : base(dbProvider, configuration, messageProvider, logger)
        {
                
        }
    }
}
