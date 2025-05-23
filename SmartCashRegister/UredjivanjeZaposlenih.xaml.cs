﻿using SmartCashRegister.Models;
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
            PasswordBox.Password = PasswordTextBox.Text;
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
                Input_JMBG.Text = row["jmbg"].ToString();
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
                PasswordTextBox.Text = PasswordBox.Password;
                PasswordTextBox.Visibility = Visibility.Visible;
                PasswordBox.Visibility = Visibility.Collapsed;

                EyeIcon.Source = new BitmapImage(new System.Uri("Resources/zatvoreno_oko.png", System.UriKind.Relative));
            }

            vidljivostLozinke = !vidljivostLozinke;
        }

        private void Button_Dodaj_Click(object sender, RoutedEventArgs e)
        {
            if(ProveriPolja())
            {
                Osoba novaOsoba = new Osoba
                {
                    Ime = Input_Ime.Text,
                    Prezime = Input_Prezime.Text,
                    Jmbg = Input_JMBG.Text,
                    Telefon = Input_Telefon.Text,
                    Username = Input_Username.Text,
                    Sifra = PasswordBox.Password,
                    Uloga = (Input_Uloga.SelectedItem as ComboBoxItem)?.Content.ToString()
                };

                bool uspeh = _uredjivanjeZaposlenihService.DodajZaposlenog(novaOsoba);

                if (uspeh)
                {
                    MessageBox.Show("Zaposleni je uspešno dodat");
                    OsveziDataGrid();
                    OcistiPolja();
                }
            }
        }
        private void OsveziDataGrid()
        {
            DataGrid_Zaposleni.ItemsSource = null;
            DataGrid_Zaposleni.ItemsSource = _uredjivanjeZaposlenihService.PrikaziSveZaposlene();
        }
        private bool ProveriPolja()
        {
            string unetaLozinka;
            if (PasswordBox.Visibility == Visibility.Visible)
                unetaLozinka = PasswordBox.Password;
            else
                unetaLozinka = PasswordTextBox.Text;

            if (Input_Ime.Text == "" ||
                Input_Prezime.Text == "" ||
                Input_JMBG.Text == "" ||
                Input_Telefon.Text == "" ||
                Input_Username.Text == "" ||
                string.IsNullOrWhiteSpace(unetaLozinka) ||
                Input_Uloga.Text == "")
            {
                MessageBox.Show("Morate popuniti SVA polja");
                return false;
            }
            return true;
        }

        private void OcistiPolja()
        {
            Input_Ime.Clear();
            Input_Prezime.Clear();
            Input_JMBG.Clear();
            Input_Telefon.Clear();
            Input_Username.Clear();
            PasswordBox.Clear();
            PasswordTextBox.Clear();
            Input_Uloga.SelectedIndex = -1;
            DataGrid_Zaposleni.SelectedItem = null;
            Button_Izmeni.Visibility = Visibility.Hidden;
            Button_Obrisi.Visibility = Visibility.Hidden;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PasswordBox.Password))
                EyeIcon.Visibility = Visibility.Collapsed;
            else
                EyeIcon.Visibility = Visibility.Visible;
        }

        private void Button_Obrisi_Click(object sender, RoutedEventArgs e)
        {
            if (DataGrid_Zaposleni.SelectedItem is Osoba selektovanaOsoba)
            {
                var potvrda = MessageBox.Show("Da li ste sigurni da želite da obrišete zaposlenog?", "Potvrda", MessageBoxButton.YesNo);
                if (potvrda != MessageBoxResult.Yes) return;

                bool uspeh = _uredjivanjeZaposlenihService.ObrisiZaposlenog(selektovanaOsoba.OsobaId);

                if (uspeh)
                {
                    MessageBox.Show("Zaposleni je obrisan");
                    OsveziDataGrid();
                    OcistiPolja();
                }
            }            
        }

        private void Button_Izmeni_Click(object sender, RoutedEventArgs e)
        {
            if (DataGrid_Zaposleni.SelectedItem is Osoba selektovanaOsoba)
            {
                bool nestoSePromenilo =
                    selektovanaOsoba.Ime != Input_Ime.Text ||
                    selektovanaOsoba.Prezime != Input_Prezime.Text ||
                    selektovanaOsoba.Jmbg != Input_JMBG.Text ||
                    selektovanaOsoba.Telefon != Input_Telefon.Text ||
                    selektovanaOsoba.Username != Input_Username.Text ||
                    selektovanaOsoba.Sifra != PasswordBox.Password ||
                    selektovanaOsoba.Uloga != (Input_Uloga.SelectedItem as ComboBoxItem)?.Content.ToString();

                if (!nestoSePromenilo)
                {
                    MessageBox.Show("Nema promena za čuvanje");
                    return;
                }

                selektovanaOsoba.Ime = Input_Ime.Text;
                selektovanaOsoba.Prezime = Input_Prezime.Text;
                selektovanaOsoba.Jmbg = Input_JMBG.Text;
                selektovanaOsoba.Telefon = Input_Telefon.Text;
                selektovanaOsoba.Username = Input_Username.Text;
                selektovanaOsoba.Sifra = PasswordBox.Password;
                selektovanaOsoba.Uloga = (Input_Uloga.SelectedItem as ComboBoxItem)?.Content.ToString();

                if (ProveriPolja())
                {
                    bool uspeh = _uredjivanjeZaposlenihService.IzmeniZaposlenog(selektovanaOsoba);

                    if (uspeh)
                    {
                        MessageBox.Show("Podaci su uspešno izmenjeni");
                        OsveziDataGrid();
                        OcistiPolja();
                    }
                }
            }
        }
    }
}
