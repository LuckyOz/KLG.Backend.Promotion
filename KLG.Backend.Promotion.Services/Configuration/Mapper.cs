using AutoMapper;
using KLG.Backend.Promotion.Models.Message;
using KLG.Backend.Promotion.Models.Request;
using KLG.Backend.Promotion.Services.Entities;

namespace KLG.Backend.Promotion.Services.Configuration;

public static class KLGMapper
{
    public static Mapper Mapper;

    /// <summary>
    /// Initializes the AutoMapper configuration.
    /// </summary>
    public static void Initialize()
    {
        var config = new MapperConfiguration(cfg =>
        {
            
        });

        Mapper = new Mapper(config);
    }

    /// <summary>
    /// Maps an object to the specified type using the configured AutoMapper instance.
    /// </summary>
    /// <typeparam name="T">The destination type.</typeparam>
    /// <param name="source">The source object to map.</param>
    /// <returns>The mapped object of type T.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the Mapper instance is not set.</exception>
    public static T Map<T>(object source)
    {
        if (Mapper == null) throw new InvalidOperationException("Mapper not set");
        return Mapper.Map<T>(source);
    }
}
