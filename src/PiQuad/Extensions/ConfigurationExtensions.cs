using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PiQuad.Extensions;

public static class ConfigurationExtensions
{
    public static T Configure<T>(this IServiceCollection services, IConfiguration configuration)
        where T : class, new()
    {
        var options = new T();
        configuration.Bind(options);

        return options;
    }

    public static T Configure<T>(this IServiceCollection services, IConfiguration configuration, string section)
        where T : class, new()
    {
        return services.Configure<T>(configuration.GetSection(section));
    }
}