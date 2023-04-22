using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PiQuad.Application.RequestHandlers;

namespace PiQuad.Extensions;

public static class HostBuilderExtensions
{
    public static void ConfigureBuilder(this IHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.Sources.Clear();

            config.SetBasePath(context.HostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
        });

        builder.ConfigureServices((context, services) =>
        {
            services.AddOptions();
            ConfigureServices(services, context.Configuration);
        });
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureGpioDaemonService(configuration);
        services.ConfigureMotorControllerService(configuration);
        services.ConfigureImuService(configuration);
        services.ConfigureCommunicationService(configuration);

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(typeof(ThrottleRequestHandler).Assembly);
        });
    }
}