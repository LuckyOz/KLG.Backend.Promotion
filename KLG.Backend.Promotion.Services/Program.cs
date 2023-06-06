
using KLG.Library.Microservice;
using KLG.Backend.Promotion.Services.Entities;
using KLG.Backend.Promotion.Services.Configuration;
using KLG.Backend.Promotion.Services.Business.Promotion;
using KLG.Backend.Promotion.Services.Business.Engine;

namespace KLG.Backend.Organization.Services;

public static class Promotion
{
    public async static Task Main()
    {
        // initialize auto-mapper
        KLGMapper.Initialize();

        // cerate new microservice builder
        var builder = new KLGMicroserviceBuilder<DefaultDbContext>();

        // Add Service
        builder.Services.AddScoped<IPromoManager, PromoManager>();
        builder.Services.AddScoped<IEngineManager, EngineManager>();
        builder.Services.AddSingleton<IPromoSetup, PromoSetup>();

        // Add Background Job
        //builder.Services.AddHostedService<PromoBackground>();

        // Add Config
        builder.RegisterConfigurationSection<PromoConfiguration>();

        // Run App
        await builder.Build().RunAsync();

    }

}
