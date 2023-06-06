
using Microsoft.AspNetCore.Mvc;
using KLG.Library.Microservice;
using KLG.Library.Microservice.Messaging;
using KLG.Library.Microservice.DataAccess;
using KLG.Library.Microservice.Configuration;
using KLG.Backend.Promotion.Services.Entities;
using KLG.Backend.Promotion.Models.Message;

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

        [NonAction]
        [KLGMessageSubscribe(nameof(PromoCreated))]
        public async Task EmployeeCreatedSubscriber(KLGMessage p)
        {
            var data = _messageProvider.GetPayloadAsync<PromoCreated>(p)
                ?? throw new InvalidOperationException("Payload is null");
        }
    }
}
