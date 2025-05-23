﻿using SmartCashRegister.Models;
using SmartCashRegister.Services.Interfaces;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

using Microsoft.Data.SqlClient;

namespace SmartCashRegister
{
    /// <summary>
    /// Interaction logic for PodesavanjeProfila.xaml
    /// </summary>
    public partial class PodesavanjeProfila : UserControl
    {
        private bool vidljivostLozinke = false;
        Osoba prijavljeni;
        private readonly IPodesavanjeProfilaService _podesavanjeProfilaService;
        private readonly IPristupBaziService _dbPristup;
        public PodesavanjeProfila(Osoba prijavljeni, IPristupBaziService dbPristup, IPodesavanjeProfilaService podesavanjeProfilaService)
        {
            InitializeComponent();
            _dbPristup = dbPristup;
            _podesavanjeProfilaService = podesavanjeProfilaService;
            EyeIcon.Visibility = Visibility.Collapsed;
            this.prijavljeni= prijavljeni;
            PrikaziTrenutnePodatke();
        }
        private bool PrikaziTrenutnePodatke()
        {
            string query = $"SELECT ime, prezime, telefon, username, sifra FROM Osoba WHERE osoba_id={prijavljeni.OsobaId}";
            DataTable dt = _dbPristup.ExecuteQuery(query);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                txtIme.Text = row["ime"].ToString();
                txtPrezime.Text = row["prezime"].ToString();
                txtTelefon.Text = row["telefon"].ToString();
                txtUsername.Text = row["username"].ToString();
                PasswordBox.Password = row["sifra"].ToString();
                PasswordTextBox.Text = PasswordBox.Password;
            }
            return true;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PasswordBox.Password))
                EyeIcon.Visibility = Visibility.Collapsed;
            else
                EyeIcon.Visibility = Visibility.Visible;
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

        private void PasswordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PasswordBox.Password=PasswordTextBox.Text;
        }

        private void btnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            if (!ProveriPolja())
                return;
            bool uspesno = _podesavanjeProfilaService.SacuvajPromeneAkoPostoje(
                prijavljeni,
                txtIme.Text,
                txtPrezime.Text,
                txtTelefon.Text,
                txtUsername.Text,
                PasswordTextBox.Text
            );

            if (uspesno)
                MessageBox.Show("Uspešno ste sačuvali promene");

        }
        private bool ProveriPolja()
        {
            if (string.IsNullOrWhiteSpace(txtIme.Text) ||
                string.IsNullOrWhiteSpace(txtPrezime.Text) ||
                string.IsNullOrWhiteSpace(txtTelefon.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(PasswordTextBox.Text))
            {
                MessageBox.Show("Sva polja moraju biti popunjena", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }
    }
}
