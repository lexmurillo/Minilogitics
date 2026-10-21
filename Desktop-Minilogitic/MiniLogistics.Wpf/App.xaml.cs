
using Microsoft.Extensions.DependencyInjection;
using MiniLogistics.Wpf.Services;
using System.Net.Http;
using System.Windows;

namespace MiniLogistics.Wpf;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var services = new ServiceCollection();

        // Cliente HTTP para Packages.Api
        services.AddHttpClient("PackagesApi", client =>
        {
            client.BaseAddress = new Uri("http://localhost:5073/");
        });

        // Cliente HTTP para Shipping.Api
        services.AddHttpClient("ShippingApi", client =>
        {
            client.BaseAddress = new Uri("http://localhost:5247/");
        });

        // Cliente HTTP para Tracking.Api
        services.AddHttpClient("TrackingApi", client =>
        {
            client.BaseAddress = new Uri("http://localhost:5265/");
        });

        // Servicio de paquetes
        services.AddTransient<PackageService>(serviceProvider =>
        {
            var factory = serviceProvider
                .GetRequiredService<IHttpClientFactory>();

            var client = factory.CreateClient("PackagesApi");

            return new PackageService(client);
        });

        // Servicio de envíos
        services.AddTransient<ShippingService>(serviceProvider =>
        {
            var factory = serviceProvider
                .GetRequiredService<IHttpClientFactory>();

            var client = factory.CreateClient("ShippingApi");

            return new ShippingService(client);
        });

        // Servicio de seguimiento
        services.AddTransient<TrackingService>(serviceProvider =>
        {
            var factory = serviceProvider
                .GetRequiredService<IHttpClientFactory>();

            var client = factory.CreateClient("TrackingApi");

            return new TrackingService(client);
        });

        // Ventana principal
        services.AddTransient<MainWindow>();

        var serviceProvider = services.BuildServiceProvider();

        var mainWindow =
            serviceProvider.GetRequiredService<MainWindow>();

        mainWindow.Show();
    }
}

