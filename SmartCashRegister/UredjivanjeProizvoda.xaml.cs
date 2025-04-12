using SmartCashRegister.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartCashRegister
{
    /// <summary>
    /// Interaction logic for UredjivanjeProizvoda.xaml
    /// </summary>
    public partial class UredjivanjeProizvoda : UserControl
    {
        private readonly IPretragaProizvodaService _pretragaProizvodaService;
        private readonly IPrikazKategorijaService _prikazKategorijaService;
        public UredjivanjeProizvoda(IPretragaProizvodaService pretragaProizvodaService, IPrikazKategorijaService prikazKategorijaService)
        {
            _pretragaProizvodaService = pretragaProizvodaService;
            _prikazKategorijaService = prikazKategorijaService;
            InitializeComponent();

            dataGridKategorije.ItemsSource = _prikazKategorijaService.PrikaziSve();
        }

        private void Button_PretraziProizvod_Click(object sender, RoutedEventArgs e)
        {
            dataGridProizvodi.Visibility = Visibility.Visible;
            if (Button_PretraziProizvod.Content.ToString() == "Pretrazi")
            {
                dataGridProizvodi.ItemsSource = _pretragaProizvodaService.PretraziProizvod(TextBox_BarKodProizvoda.Text, TextBox_NazivProizvoda.Text, TextBox_KategorijaProizvoda.Text);
            }
            else
            {
                dataGridProizvodi.ItemsSource = _pretragaProizvodaService.PrikaziSveProizvode();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBox_BarKodProizvoda.Text != string.Empty || TextBox_KategorijaProizvoda.Text != string.Empty || TextBox_NazivProizvoda.Text != string.Empty)
            {
                Button_PretraziProizvod.Content = "Pretrazi";
            }
            else
            {
                Button_PretraziProizvod.Content = "Prikazi sve";
            }
        }
    }
}
