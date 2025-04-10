using SmartCashRegister.Models;
using SmartCashRegister.Services.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace SmartCashRegister
{
    /// <summary>
    /// Interaction logic for ProizvodiPrikaz.xaml
    /// </summary>
    public partial class ProizvodiPrikaz : UserControl
    {
        private readonly IPretragaProizvodaService _pretragaProizvodaService;
        private readonly IPrikazKategorijaService _prikazKategorijaService;
        public ProizvodiPrikaz(IPretragaProizvodaService pretragaProizvodaService, IPrikazKategorijaService prikazKategorijaService)
        {
            _pretragaProizvodaService= pretragaProizvodaService;
            _prikazKategorijaService= prikazKategorijaService;
            InitializeComponent();
            dataGridKategorije.ItemsSource = _prikazKategorijaService.PrikaziSve();
            
        }

        private void Button_PretraziProizvod_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            dataGridProizvodi.Visibility = Visibility.Visible;
            if (Button_PretraziProizvod.Content.ToString()=="Pretrazi")
            {
                //prikaz po filterima
            }
            else
            {
                dataGridProizvodi.ItemsSource=_pretragaProizvodaService.PrikaziSveProizvode();
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
