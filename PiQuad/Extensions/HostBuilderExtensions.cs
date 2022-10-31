using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PiQuad.Extensions;

public static class HostBuilderExtensions
{
    public static void ConfigureBuilder(this IHostBuilder builder)
    {
        builder.ConfigureServices((context, services) =>
        {
            ConfigureServices(services, context.Configuration);
        });
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureGpioDaemonService(configuration);
        services.ConfigureMotorControllerService(configuration);
        services.ConfigureImuService(configuration);
        services.ConfigurePiQuadHostedService();
    }
}