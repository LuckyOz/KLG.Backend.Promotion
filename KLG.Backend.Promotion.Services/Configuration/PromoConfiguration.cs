using KLG.Library.Microservice.Configuration.Interface;

namespace KLG.Backend.Promotion.Services.Configuration;

public class PromoConfiguration : IKLGConfigurationSection
{
    //Path for get Config Environment at appsettings.json
    public string Path => "PromotionConfiguration";

    //Class for Get and Set Config ar Envitonement
    public int MinimumAge { get; private set; }

    public PromoConfiguration(int minimumAge)
    {
        MinimumAge = minimumAge;
    }

    public PromoConfiguration() { }
}
