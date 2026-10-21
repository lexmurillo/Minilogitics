
using MiniLogistics.Wpf.Services;
using MiniLogistics.Wpf.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MiniLogistics.Wpf;

public partial class MainWindow : Window
{
    private readonly PackageService _packageService;
    private readonly ShippingService _shippingService;
    private readonly TrackingService _trackingService;

    public MainWindow(
        PackageService packageService,
        ShippingService shippingService,
        TrackingService trackingService)
    {
        InitializeComponent();

        _packageService = packageService;
        _shippingService = shippingService;
        _trackingService = trackingService;

        Loaded += MainWindow_Loaded;

        PaquetesDataGrid.SelectionChanged +=
            PaquetesDataGrid_SelectionChanged;
    }

    // Cargar paquetes y envíos al iniciar
    private async void MainWindow_Loaded(
        object sender,
        RoutedEventArgs e)
    {
        try
        {
            await CargarPaquetesAsync();
            await CargarEnviosAsync();
        }
        catch (HttpRequestException ex)
        {
            MessageBox.Show(
                $"No fue posible conectar con una API.\n\n{ex.Message}\n\n" +
                "Verifica que ambas API estén ejecutándose y que los puertos sean correctos.",
                "Error de conexión",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Error al cargar los datos:\n{ex.Message}",
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    // =========================
    // PAQUETES
    // =========================

    private async Task CargarPaquetesAsync()
    {
        var packages = await _packageService.GetPackagesAsync();

        PaquetesDataGrid.ItemsSource = null;
        PaquetesDataGrid.ItemsSource = packages;
    }

    private void PaquetesDataGrid_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e)
    {
        if (PaquetesDataGrid.SelectedItem is not Package paquete)
            return;

        IdTextBlock.Text = paquete.Id.ToString();
        DescripcionTextBlock.Text = paquete.Description;
        PesoTextBlock.Text = paquete.Weight.ToString();
        OrigenTextBlock.Text = paquete.Origin;
        DestinoTextBlock.Text = paquete.Destination;
        FechaTextBlock.Text = paquete.CreatedAt.ToString();
    }

    private async void NuevoPaquete_Click(
        object sender,
        RoutedEventArgs e)
    {
        var ventana = new NewPackageWindow
        {
            Owner = this
        };

        if (ventana.ShowDialog() != true ||
            ventana.CreatedPackage is not Package paquete)
        {
            return;
        }

        try
        {
            await _packageService.CreatePackageAsync(paquete);
            await CargarPaquetesAsync();

            MessageBox.Show("Paquete creado correctamente.");
        }
        catch (HttpRequestException ex)
        {
            MessageBox.Show(
                $"No fue posible conectar con la API de paquetes.\n{ex.Message}");
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Error al crear el paquete: {ex.Message}");
        }
    }

    private async void Actualizar_Click(
        object sender,
        RoutedEventArgs e)
    {
        try
        {
            await CargarPaquetesAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Error al actualizar los paquetes: {ex.Message}");
        }
    }

    private async void Eliminar_Click(
        object sender,
        RoutedEventArgs e)
    {
        if (PaquetesDataGrid.SelectedItem is not Package paquete)
        {
            MessageBox.Show("Selecciona un paquete para eliminar.");
            return;
        }

        var resultado = MessageBox.Show(
            $"¿Seguro que deseas eliminar el paquete {paquete.Id}?",
            "Confirmar eliminación",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (resultado != MessageBoxResult.Yes)
            return;

        try
        {
            await _packageService.DeletePackageAsync(paquete.Id);
            await CargarPaquetesAsync();

            MessageBox.Show("Paquete eliminado correctamente.");
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Error al eliminar el paquete: {ex.Message}");
        }
    }
    private async void BuscarPaquete_Click(object sender, RoutedEventArgs e)
    {
        if (!int.TryParse(BuscarPaqueteIdTextBox.Text, out int id) || id <= 0)
        {
            MessageBox.Show("Introduce un ID de paquete válido.");
            return;
        }

        try
        {
            var paquete = await _packageService.GetPackageByIdAsync(id);

            if (paquete is null)
            {
                MessageBox.Show($"No existe un paquete con el ID {id}.");
                return;
            }

            // Mostrar el paquete encontrado en la tabla.
            PaquetesDataGrid.ItemsSource = new List<Package> { paquete };

            // Mostrar sus detalles en el panel derecho.
            IdTextBlock.Text = paquete.Id.ToString();
            DescripcionTextBlock.Text = paquete.Description;
            PesoTextBlock.Text = paquete.Weight.ToString();
            OrigenTextBlock.Text = paquete.Origin;
            DestinoTextBlock.Text = paquete.Destination;
            FechaTextBlock.Text = paquete.CreatedAt.ToString();
        }
        catch (HttpRequestException ex)
        {
            MessageBox.Show(
                $"Error al consultar el paquete.\n\n{ex.Message}");
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Ocurrió un error inesperado.\n\n{ex.Message}");
        }
    }

    // =========================
    // ENVÍOS
    // =========================

    private async Task CargarEnviosAsync()
    {
        var shipments = await _shippingService.GetShipmentsAsync();

        EnviosDataGrid.ItemsSource = null;
        EnviosDataGrid.ItemsSource = shipments;
    }

    private async void CrearEnvio_Click(
        object sender,
        RoutedEventArgs e)
    {
        if (!int.TryParse(PackageIdTextBox.Text, out int packageId)
            || packageId <= 0)
        {
            MessageBox.Show("Introduce un ID de paquete válido.");
            return;
        }

        try
        {
            var nuevoEnvio = new Shipment
            {
                PackageId = packageId
            };

            await _shippingService.CreateShipmentAsync(nuevoEnvio);
            await CargarEnviosAsync();

            PackageIdTextBox.Clear();

            MessageBox.Show("Envío creado correctamente.");
        }
        catch (HttpRequestException ex)
        {
            MessageBox.Show(
                $"No se pudo crear el envío.\n\n{ex.Message}\n\n" +
                "Comprueba que el paquete exista y que no tenga ya un envío asociado.");
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Error al crear el envío: {ex.Message}");
        }
    }

    private async void CambiarEstadoEnvio_Click(
        object sender,
        RoutedEventArgs e)
    {
        if (EnviosDataGrid.SelectedItem is not Shipment envio)
        {
            MessageBox.Show("Selecciona un envío de la tabla.");
            return;
        }

        if (EstadoEnvioComboBox.SelectedItem is not ComboBoxItem elemento
            || elemento.Content is not string nuevoEstado)
        {
            MessageBox.Show("Selecciona un estado válido.");
            return;
        }
        if (CiudadTrackingComboBox.SelectedItem is not ComboBoxItem ciudadItem
                 ||ciudadItem.Content is not string ciudad)
        {
            MessageBox.Show("Selecciona una ciudad válida.");
            return;
        }

        if (envio.Status == nuevoEstado)
        {
            MessageBox.Show(
                "El envío ya tiene ese estado.");
            return;
        }

        try
        {
            await _shippingService.UpdateShipmentStatusAsync(
                envio.Id,
                nuevoEstado,
                ciudad);

            await CargarEnviosAsync();

            MessageBox.Show(
                "Estado del envío actualizado correctamente.");
        }
        catch (HttpRequestException ex)
        {
            MessageBox.Show(
                $"No se pudo cambiar el estado.\n\n{ex.Message}");
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Error al cambiar el estado: {ex.Message}");
        }
    }

    private async void ActualizarEnvios_Click(
        object sender,
        RoutedEventArgs e)
    {
        try
        {
            await CargarEnviosAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Error al actualizar los envíos: {ex.Message}");
        }
    }
    private async void BuscarEnvio_Click(object sender, RoutedEventArgs e)
    {
        if (!int.TryParse(BuscarEnvioIdTextBox.Text, out int id) || id <= 0)
        {
            MessageBox.Show("Introduce un ID de envío válido.");
            return;
        }

        try
        {
            var envio = await _shippingService.GetShipmentByIdAsync(id);

            if (envio is null)
            {
                MessageBox.Show($"No existe un envío con el ID {id}.");
                return;
            }

            EnviosDataGrid.ItemsSource = new List<Shipment> { envio };
        }
        catch (HttpRequestException ex)
        {
            MessageBox.Show($"Error al consultar el envío.\n\n{ex.Message}");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ocurrió un error inesperado.\n\n{ex.Message}");
        }
    }

    private async void EliminarEnvio_Click(
        object sender,
        RoutedEventArgs e)
    {
        if (EnviosDataGrid.SelectedItem is not Shipment shipment)
        {
            MessageBox.Show(
                "Selecciona un envío para eliminar.",
                "Eliminar envío",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        var confirmacion = MessageBox.Show(
            $"¿Seguro que deseas eliminar el envío {shipment.Id}?",
            "Confirmar eliminación",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (confirmacion != MessageBoxResult.Yes)
            return;

        try
        {
            var eliminado =
                await _shippingService.DeleteShipmentAsync(shipment.Id);

            if (!eliminado)
            {
                MessageBox.Show("No se pudo eliminar el envío.");
                return;
            }

            await CargarEnviosAsync();

            MessageBox.Show(
                "El envío se eliminó correctamente.",
                "Eliminar envío",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Ocurrió un error al eliminar el envío:\n{ex.Message}",
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private async void ConsultarTracking_Click(
        object sender,
        RoutedEventArgs e)
    {
        if (!int.TryParse(
                PackageIdTrackingTextBox.Text,
                out int packageId)
            || packageId <= 0)
        {
            MessageBox.Show(
                "Introduce un ID de paquete válido.",
                "Seguimiento",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        try
        {
            var events = await _trackingService
                .GetTrackingByPackageIdAsync(packageId);

            TrackingDataGrid.ItemsSource = null;
            TrackingDataGrid.ItemsSource = events;

            if (events.Count == 0)
            {
                MessageBox.Show(
                    $"El paquete {packageId} no tiene eventos de seguimiento.",
                    "Seguimiento",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }
        catch (HttpRequestException ex)
        {
            MessageBox.Show(
                $"No se pudo consultar Tracking.Api.\n\n{ex.Message}\n\n" +
                "Comprueba que Tracking.Api esté ejecutándose en el puerto 5265.",
                "Error de conexión",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Error al consultar el seguimiento:\n{ex.Message}",
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }


}



