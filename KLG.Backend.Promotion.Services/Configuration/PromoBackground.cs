
using KLG.Backend.Promotion.Services.Business.Engine;

namespace KLG.Backend.Promotion.Services.Configuration
{
    public class PromoBackground : BackgroundService
    {
        private readonly IServiceScopeFactory _factory;

        public PromoBackground(IServiceScopeFactory factory)
        {
            _factory = factory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try {
                await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
                IPromoSetup setupPromoHelper = asyncScope.ServiceProvider.GetRequiredService<IPromoSetup>();
                IEngineManager setupEngineManager = asyncScope.ServiceProvider.GetRequiredService<IEngineManager>();

                setupPromoHelper.RefreshWorkflow(await setupEngineManager.GetWorkflow());
            } finally { }
        }
    }
}
