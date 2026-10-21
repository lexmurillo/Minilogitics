
using MiniLogistics.Wpf.Models;
using System.Globalization;
using System.Windows;

namespace MiniLogistics.Wpf;

public partial class NewPackageWindow : Window
{
    public Package? CreatedPackage { get; private set; }

    public NewPackageWindow()
    {
        InitializeComponent();
    }

    private void Guardar_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(DescripcionTextBox.Text) ||
            string.IsNullOrWhiteSpace(PesoTextBox.Text) ||
            string.IsNullOrWhiteSpace(OrigenTextBox.Text) ||
            string.IsNullOrWhiteSpace(DestinoTextBox.Text))
        {
            MessageBox.Show("Completa todos los campos.");
            return;
        }

        if (!decimal.TryParse(
                PesoTextBox.Text,
                NumberStyles.Number,
                CultureInfo.CurrentCulture,
                out decimal peso) || peso <= 0)
        {
            MessageBox.Show("Ingresa un peso válido mayor que cero.");
            return;
        }

        CreatedPackage = new Package
        {
            Description = DescripcionTextBox.Text.Trim(),
            Weight = peso,
            Origin = OrigenTextBox.Text.Trim(),
            Destination = DestinoTextBox.Text.Trim()
        };

        DialogResult = true;
    }
}
