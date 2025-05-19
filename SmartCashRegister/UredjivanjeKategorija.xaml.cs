using SmartCashRegister.Models;
using SmartCashRegister.Services.Interfaces;
using System.Windows;
using System.Windows.Controls;
using System.Data;

namespace SmartCashRegister
{
    /// <summary>
    /// Interaction logic for UredjivanjeKategorija.xaml
    /// </summary>
    public partial class UredjivanjeKategorija : UserControl
    {
        private readonly IUredjivanjeKategorijaService _uredjivanjeKategorijaService;
        private readonly IPristupBaziService _dbPristup;
        public UredjivanjeKategorija(IUredjivanjeKategorijaService uredjivanjeKategorijaService , IPristupBaziService dbPristup)
        {
            _uredjivanjeKategorijaService = uredjivanjeKategorijaService;
            InitializeComponent();
            _dbPristup = dbPristup;
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

            Button_Izmeni.Visibility = Visibility.Hidden;
            Button_Obrisi.Visibility = Visibility.Hidden;
        }

        private void Button_Obrisi_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridKategorije.SelectedItem is Kategorija selektovanaKategorija)
            {
                var potvrda = MessageBox.Show("Da li ste sigurni da želite da obrišete kategoriju?", "Potvrda", MessageBoxButton.YesNo);
                if (potvrda != MessageBoxResult.Yes) return;

                bool uspeh = _uredjivanjeKategorijaService.ObrisiKategoriju(selektovanaKategorija.KategorijaId);

                if (uspeh)
                {
                    MessageBox.Show("Kategorija je obrisana");
                    OsveziDataGrid();
                    OcistiPolja();
                }
            }
        }

        private void dataGridKategorije_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridKategorije.SelectedItem is Kategorija selektovanaKategorija)
            {
                PrikaziSelektovanu(selektovanaKategorija.KategorijaId);
                Button_Obrisi.Visibility = Visibility.Visible;
                Button_Izmeni.Visibility = Visibility.Visible;

            }
        }
        private bool PrikaziSelektovanu(int kategorijaId)
        {
            string query = $"SELECT * FROM Kategorija WHERE kategorija_id={kategorijaId}";
            DataTable dt = _dbPristup.ExecuteQuery(query);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                Input_Naziv.Text = row["naziv"].ToString();
            }
            return true;
        }
    }
}
