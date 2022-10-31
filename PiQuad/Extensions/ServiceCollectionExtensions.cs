using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

    public static void ConfigureMotorControllerService(this IServiceCollection services, IConfiguration configuration)
    {
        var config = services.Configure<MotorControllerServiceSettings>(configuration, MotorControllerServiceSettingsConfigurationKey);
        services.AddSingleton(config);
        services.AddSingleton<IMotorControllerService, MotorControllerService>();
    }
    
    public static void ConfigureImuService(this IServiceCollection services, IConfiguration configuration)
    {
        var config = services.Configure<ImuSettings>(configuration, ImuServiceSettingsConfigurationKey);
        services.AddSingleton(config);
        services.AddSingleton<IImu, Mpu6050>();
        services.AddSingleton<IImuService, ImuService>();
    }

    public static void ConfigureGpioDaemonService(this IServiceCollection services, IConfiguration configuration)
    {
        var config = services.Configure<GpioDaemonServiceSettings>(configuration, GpioDaemonServiceConfigurationKey);
        services.AddSingleton(config);
        services.AddSingleton<IGpioDaemonService, GpioDaemonService>();
    }

    public static void ConfigurePiQuadHostedService(this IServiceCollection services)
    {
        services.AddSingleton<PiQuadControllerService>();
        services.AddHostedService<PiQuadHostedService>();
    }
}