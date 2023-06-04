using KLG.Backend.Promotion.Services.Business.Promotion;
using KLG.Backend.Promotion.Services.Configuration;
using KLG.Backend.Promotion.Services.Entities;
using KLG.Library.Microservice;

namespace KLG.Backend.Organization.Services;

public static class Promotion
{
    public async static Task Main(string[] args)
    {
        // initialize auto-mapper
        KLGMapper.Initialize();

        // cerate new microservice builder
        var builder = new KLGMicroserviceBuilder<DefaultDbContext>();

        // add your own services
        builder.Services.AddScoped<IPromotionManager, PromotionManager>();

        // add your own configuration classes
        // you can add more than one custom configurations
        builder.RegisterConfigurationSection<PromotionConfiguration>();

        // run the microservice
        await builder.Build().RunAsync();

    }

}
