using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PiQuad.Application.CommunicationService;
using PiQuad.Application.CommunicationService.Settings;
using PiQuad.Application.Fake;
using PiQuad.Application.GpioDaemonService;
using PiQuad.Application.GpioDaemonService.Settings;
using PiQuad.Application.ImuService;
using PiQuad.Application.ImuService.Settings;
using PiQuad.Application.MotorControllerService;
using PiQuad.Application.MotorControllerService.Settings;
using PiQuad.Application.PiQuadHostedService;

namespace PiQuad.Extensions;

public static class ServiceCollectionExtensions
{
    private const string MotorControllerServiceSettingsConfigurationKey = "MotorControllerService";
    private const string ImuServiceSettingsConfigurationKey = "ImuService";
    private const string GpioDaemonServiceConfigurationKey = "GpioDaemonService";
    private const string WebsocketConfigurationKey = "Websocket";

    public static void ConfigureMotorControllerService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MotorControllerServiceSettings>(options => 
            configuration.GetSection(MotorControllerServiceSettingsConfigurationKey).Bind(options));
        services.AddSingleton<IMotorControllerService, MotorControllerService>();
    }

    public static void ConfigureImuService(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.GetSection("Overrides").GetValue<bool>("OverrideImuWithFake"))
        {
            services.AddSingleton<IImu, FakeImu>();
        }
        else
        {
            services.Configure<ImuSettings>(options => 
                configuration.GetSection(ImuServiceSettingsConfigurationKey).Bind(options));
            services.AddSingleton<IImu, Mpu6050>();
        }

        services.AddSingleton<IImuService, ImuService>();
    }

    public static void ConfigureGpioDaemonService(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.GetSection("Overrides").GetValue<bool>("OverrideGpioDaemonWithFake"))
        {
            services.AddSingleton<IGpioDaemonService, FakeGpioDaemonService>();
        }
        else
        {
            services.Configure<GpioDaemonServiceSettings>(options => 
                configuration.GetSection(GpioDaemonServiceConfigurationKey).Bind(options));
            services.AddSingleton<IGpioDaemonService, GpioDaemonService>();
        }
    }

    public static void ConfigureCommunicationService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<WebsocketSettings>(options => 
            configuration.GetSection(WebsocketConfigurationKey).Bind(options));
        services.AddSingleton<ICommunicationService, WebsocketCommunicationService>();
    }

    public static void ConfigurePiQuadHostedService(this IServiceCollection services)
    {
        services.AddHostedService<CommunicationHostedService>();
        services.AddHostedService<ImuHostedService>();
        services.AddSingleton<PiQuadControllerService>();
    }
}