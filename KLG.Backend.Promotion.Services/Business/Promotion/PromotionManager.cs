using KLG.Backend.Promotion.Services.Entities;
using KLG.Library.Microservice.Configuration;
using KLG.Library.Microservice.DataAccess;
using KLG.Library.Microservice.Messaging;

namespace KLG.Backend.Promotion.Services.Business.Promotion
{
    public class PromotionManager : IPromotionManager
    {
        private readonly IKLGDbProvider<DefaultDbContext> _dbProvider;
        private readonly IKLGMessagingProvider _messageProvider;
        private readonly IKLGConfiguration _configuration;

        public PromotionManager(IKLGDbProvider<DefaultDbContext> dbProvider,
        IKLGMessagingProvider messageProvider, IKLGConfiguration configuration)
        {
            _dbProvider = dbProvider;
            _messageProvider = messageProvider;
            _configuration = configuration;
        }
    }
}
