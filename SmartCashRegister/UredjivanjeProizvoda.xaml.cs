using SmartCashRegister.Models;
using SmartCashRegister.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
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
        private readonly IPristupBaziService _dbPristup;
        public UredjivanjeProizvoda(IPretragaProizvodaService pretragaProizvodaService, IPristupBaziService dbPristup)
        {
            _pretragaProizvodaService = pretragaProizvodaService;
            InitializeComponent();
            _dbPristup = dbPristup;
            UcitajKategorije();
        }

        private void Button_PretraziProizvod_Click(object sender, RoutedEventArgs e)
        {
            dataGridProizvodi.Visibility = Visibility.Visible;
            if (Button_PretraziProizvod.Content.ToString() == "Pretrazi")
            {
                dataGridProizvodi.ItemsSource = _pretragaProizvodaService.PretraziProizvod(TextBox_BarKodProizvoda.Text);
            }
            else
            {
                dataGridProizvodi.ItemsSource = _pretragaProizvodaService.PrikaziSveProizvode();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBox_BarKodProizvoda.Text != string.Empty)
            {
                Button_PretraziProizvod.Content = "Pretrazi";
            }
            else
            {
                Button_PretraziProizvod.Content = "Prikazi sve";
            }
        }

        private void dataGridProizvodi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridProizvodi.SelectedItem is Proizvod selektovaniProizvod)
            {
                PrikaziSelektovani(selektovaniProizvod.ProizvodId);
                Button_Obrisi.Visibility = Visibility.Visible;
                Button_Izmeni.Visibility = Visibility.Visible;
            }
        }
        public bool PrikaziSelektovani(int proizvodId)
        {
            string query = $"SELECT * FROM Proizvod WHERE proizvod_id={proizvodId}";
            DataTable dt = _dbPristup.ExecuteQuery(query);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                Input_Naziv.Text = row["naziv"].ToString();
                Input_Cena.Text = row["cena"].ToString();
                Input_Kolicina.Text = row["kolicina"].ToString();
                Input_Barkod.Text = row["barkod"].ToString();
                ComboBox_Kategorija.SelectedValue = Convert.ToInt32(row["kategorija_id"]);
            }
            return true;
        }
        private void UcitajKategorije()
        {
            string query = "SELECT * FROM Kategorija";
            DataTable dt = _dbPristup.ExecuteQuery(query);

            List<Kategorija> kategorije = new List<Kategorija>();

            foreach (DataRow row in dt.Rows)
            {
                kategorije.Add(new Kategorija
                {
                    KategorijaId = Convert.ToInt32(row["kategorija_id"]),
                    Naziv = row["naziv"].ToString()
                });
            }

            ComboBox_Kategorija.ItemsSource = kategorije;
            ComboBox_Kategorija.DisplayMemberPath = "Naziv";
            ComboBox_Kategorija.SelectedValuePath = "KategorijaId";
        }

    }
}
