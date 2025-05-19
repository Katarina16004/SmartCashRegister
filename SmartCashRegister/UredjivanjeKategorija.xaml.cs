using SmartCashRegister.Models;
using SmartCashRegister.Services;
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
    /// Interaction logic for UredjivanjeKategorija.xaml
    /// </summary>
    public partial class UredjivanjeKategorija : UserControl
    {
        private readonly IUredjivanjeKategorijaService _uredjivanjeKategorijaService;
        public UredjivanjeKategorija(IUredjivanjeKategorijaService uredjivanjeKategorijaService)
        {
            _uredjivanjeKategorijaService = uredjivanjeKategorijaService;
            InitializeComponent();
        }

        private void Button_Pretrazi_Click(object sender, RoutedEventArgs e)
        {
            dataGridKategorije.Visibility = Visibility.Visible;
            if (Button_Pretrazi.Content.ToString() == "Pretrazi")
            {
                dataGridKategorije.ItemsSource = _uredjivanjeKategorijaService.PretraziKategoriju(TextBox_Naziv.Text);
            }
            else
            {
                dataGridKategorije.ItemsSource = _uredjivanjeKategorijaService.PrikaziSveKategorije();
            }
        }

        private void TextBox_Naziv_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBox_Naziv.Text != string.Empty)
            {
                Button_Pretrazi.Content = "Pretrazi";
            }
            else
            {
                Button_Pretrazi.Content = "Prikazi sve";
            }
        }

        private void Button_Dodaj_Click(object sender, RoutedEventArgs e)
        {
            if (ProveriPolja())
            {
                Kategorija novaKat = new Kategorija
                {
                    Naziv = Input_Naziv.Text
                };

                bool uspeh = _uredjivanjeKategorijaService.DodajKategoriju(novaKat);

                if (uspeh)
                {
                    MessageBox.Show("Kategorija je uspešno dodata");
                    OsveziDataGrid();
                    OcistiPolja();
                }
            }
        }
        private void OsveziDataGrid()
        {
            dataGridKategorije.ItemsSource = null;
            dataGridKategorije.ItemsSource = _uredjivanjeKategorijaService.PrikaziSveKategorije();
        }
        private bool ProveriPolja()
        {
            
            if (Input_Naziv.Text == "")
            {
                MessageBox.Show("Morate popuniti polje");
                return false;
            }
            return true;
        }

        private void OcistiPolja()
        {
            Input_Naziv.Clear();
            dataGridKategorije.SelectedItem = null;
        }
    }
}
