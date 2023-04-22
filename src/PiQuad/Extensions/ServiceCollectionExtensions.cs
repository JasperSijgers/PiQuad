using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PiQuad.Application.Fake;
using PiQuad.Application.MotorControllerService;
using PiQuad.Application.MotorControllerService.Settings;
using PiQuad.Application.Services;
using PiQuad.Application.SignalR;
using PiQuad.Infrastructure.GpioDaemonService;
using PiQuad.Infrastructure.GpioDaemonService.Settings;
using PiQuad.Infrastructure.ImuService;
using PiQuad.Infrastructure.ImuService.Settings;
using PiQuad.Infrastructure.SignalR;

namespace PiQuad.Extensions;

public static class ServiceCollectionExtensions
{
    private const string MotorControllerServiceSettingsConfigurationKey = "MotorControllerService";
    private const string ImuServiceSettingsConfigurationKey = "ImuService";
    private const string GpioDaemonServiceConfigurationKey = "GpioDaemonService";
    private const string SignalRConfigurationKey = "SignalR";

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
            services.Configure<ImuSettings>(options => configuration.GetSection(ImuServiceSettingsConfigurationKey).Bind(options));
            services.AddSingleton<IImu, Mpu6050>();
        }

        services.AddSingleton<IImuService, ImuService>();
        services.AddHostedService<ImuHostedService>();
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
        services.Configure<SignalRSettings>(options => configuration.GetSection(SignalRConfigurationKey).Bind(options));
        services.AddSingleton<ISignalRConnectionFactory, SignalRConnectionFactory>();
        services.AddHostedService<SignalRHostedService>();
    }
}