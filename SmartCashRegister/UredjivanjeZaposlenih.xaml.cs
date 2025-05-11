using SmartCashRegister.Models;
using SmartCashRegister.Services;
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
    /// Interaction logic for UrediZaposlene.xaml
    /// </summary>
    public partial class UrediZaposlene : UserControl
    {
        private readonly IUredjivanjeZaposlenihService _uredjivanjeZaposlenihService;

        private readonly IPristupBaziService _dbPristup;
        private bool vidljivostLozinke = false;
        public UrediZaposlene(IUredjivanjeZaposlenihService uredjivanjeZaposlenihService, IPristupBaziService dbPristup)
        {
            _uredjivanjeZaposlenihService = uredjivanjeZaposlenihService;
            _dbPristup = dbPristup;
            InitializeComponent();
        }

        private void Button_Pretrazi_Click(object sender, RoutedEventArgs e)
        {
            DataGrid_Zaposleni.Visibility = Visibility.Visible;
            if (Button_Pretrazi.Content.ToString() == "Pretrazi")
            {
                DataGrid_Zaposleni.ItemsSource = _uredjivanjeZaposlenihService.PretraziZaposlenog(TextBox_Ime.Text, TextBox_Prezime.Text, TextBox_Username.Text);
            }
            else
            {
                DataGrid_Zaposleni.ItemsSource = _uredjivanjeZaposlenihService.PrikaziSveZaposlene();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBox_Ime.Text != string.Empty || TextBox_Prezime.Text != string.Empty || TextBox_Username.Text != string.Empty)
            {
                Button_Pretrazi.Content = "Pretrazi";
            }
            else
            {
                Button_Pretrazi.Content = "Prikazi sve";
            }
        }

        private void DataGrid_Zaposleni_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGrid_Zaposleni.SelectedItem is Osoba selektovanaOsoba)
            {
                PrikaziSelektovanog(selektovanaOsoba.OsobaId);
                vidljivostLozinke = false;
                PasswordTextBox.Visibility = Visibility.Collapsed;
                PasswordBox.Visibility = Visibility.Visible;
                EyeIcon.Source = new BitmapImage(new System.Uri("Resources/otvoreno_oko.png", System.UriKind.Relative));
                Button_Obrisi.Visibility = Visibility.Visible;
                Button_Izmeni.Visibility = Visibility.Visible;

            }
        }
        public bool PrikaziSelektovanog(int osobaId)
        {
            string query = $"SELECT * FROM Osoba WHERE osoba_id={osobaId}";
            DataTable dt = _dbPristup.ExecuteQuery(query);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                Input_Ime.Text = row["ime"].ToString();
                Input_Prezime.Text = row["prezime"].ToString();
                Input_JMBG.Text = row["prezime"].ToString();
                Input_Telefon.Text = row["telefon"].ToString();
                Input_Username.Text = row["username"].ToString();
                PasswordBox.Password = row["sifra"].ToString();
                PasswordTextBox.Text = PasswordBox.Password;
                Input_Uloga.Text = row["uloga"].ToString() ;
            }
            return true;
        }

        private void EyeIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (vidljivostLozinke)
            {
                PasswordTextBox.Visibility = Visibility.Collapsed;
                PasswordBox.Visibility = Visibility.Visible;

                EyeIcon.Source = new BitmapImage(new System.Uri("Resources/otvoreno_oko.png", System.UriKind.Relative));
            }
            else
            {
                PasswordTextBox.Visibility = Visibility.Visible;
                PasswordBox.Visibility = Visibility.Collapsed;

                EyeIcon.Source = new BitmapImage(new System.Uri("Resources/zatvoreno_oko.png", System.UriKind.Relative));
            }

            vidljivostLozinke = !vidljivostLozinke;
        }
    }
}
