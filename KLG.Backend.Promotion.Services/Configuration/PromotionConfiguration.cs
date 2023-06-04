using KLG.Library.Microservice.Configuration.Interface;

namespace KLG.Backend.Promotion.Services.Configuration;

public class PromotionConfiguration : IKLGConfigurationSection
{
    //Path for get Config Environment at appsettings.json
    public string Path => "PromotionConfiguration";

    //Class for Get and Set Config ar Envitonement
    public int MinimumAge { get; private set; }

    public PromotionConfiguration(int minimumAge)
    {
        MinimumAge = minimumAge;
    }

    public PromotionConfiguration() { }
}
