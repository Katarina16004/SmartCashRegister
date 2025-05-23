﻿using SmartCashRegister.Models;
using SmartCashRegister.Services.Interfaces;
using System.Diagnostics.Eventing.Reader;
using System.Windows;
using System.Windows.Controls;

namespace SmartCashRegister
{
    /// <summary>
    /// Interaction logic for RacuniPrikaz.xaml
    /// </summary>
    public partial class RacuniPrikaz : UserControl
    {
        private readonly IPretragaRacunaService _pretragaRacunaService;
        private readonly IUpravljanjeRacunomService _upravljanjeRacunomService;
        private Osoba korisnik;
        public RacuniPrikaz(IPretragaRacunaService pretragaRacunaService, Osoba prijavljeni, IUpravljanjeRacunomService upravljanjeRacunomService)
        {
            _pretragaRacunaService=pretragaRacunaService;
            _upravljanjeRacunomService = upravljanjeRacunomService;
            
            InitializeComponent();
            korisnik = prijavljeni;
            
        }
        private void AzurirajTextButtona()
        {
            if (TextBox_IdRacuna.Text != string.Empty || TextBox_UsernameProdavca.Text != string.Empty || DP_DatumRacuna.SelectedDate != null)
            {
                Button_PretraziRacun.Content = "Pretrazi";
            }
            else
            {
                Button_PretraziRacun.Content = "Prikazi sve";
            }
        }
        private void DP_DatumRacuna_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DP_DatumRacuna.SelectedDate != null)
                Button_ResetujDatum.Visibility= Visibility.Visible;
            else
                Button_ResetujDatum.Visibility = Visibility.Hidden;
            AzurirajTextButtona();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            AzurirajTextButtona();
        }

        private void Button_PretraziRacun_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            dataGridRacuni.Visibility = Visibility.Visible;
            if (Button_PretraziRacun.Content.ToString() == "Pretrazi")
            {
                dataGridRacuni.ItemsSource = _pretragaRacunaService.PretraziRacun(TextBox_IdRacuna.Text, TextBox_UsernameProdavca.Text, DP_DatumRacuna.SelectedDate);
            }
            else
            {
                dataGridRacuni.ItemsSource = _pretragaRacunaService.PrikaziSveRacune();
            }
        }

        private void Button_RacunUPdf_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var racun = (Racun)dataGridRacuni.SelectedItem;
            if (racun != null)
            {
                _upravljanjeRacunomService.IzveziUPDF(racun);
            }
        }

        private void Button_Storniraj_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var racun = (Racun)dataGridRacuni.SelectedItem;
            if (racun != null)
            {
                if (_upravljanjeRacunomService.StornirajRacun(racun))
                {
                    MessageBox.Show("Uspešno storniran račun");
                    dataGridRacuni.Visibility = Visibility.Hidden;
                    dataGridRacun.Visibility = Visibility.Hidden;
                }
                else
                    MessageBox.Show("Greška pri storniranju računa");
            }
        }

        private void Button_ObrisiRacun_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var racun = (Racun)dataGridRacuni.SelectedItem;
            if (racun != null)
            {
                var potvrda = MessageBox.Show("Da li ste sigurni da želite da obrišete račun?", "Potvrda", MessageBoxButton.YesNo);
                if (potvrda != MessageBoxResult.Yes) return;

                if (!_upravljanjeRacunomService.ObrisiRacun(racun))
                    MessageBox.Show("Greška pri brisanju računa");
                else
                {
                    dataGridRacuni.Visibility = Visibility.Hidden;
                    dataGridRacun.Visibility = Visibility.Hidden;
                }
            }
        }

        private void Button_ResetujDatum_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DP_DatumRacuna.SelectedDate = null;
        }

        private void dataGridRacuni_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridRacuni.SelectedItem is Racun selektovaniRacun)
            {
                if (korisnik.Uloga == "administrator")
                    Button_ObrisiRacun.Visibility = Visibility.Visible;
                else
                    Button_ObrisiRacun.Visibility = Visibility.Hidden;

                var stavke = _pretragaRacunaService.PrikaziStavkeRacuna(selektovaniRacun.RacunId);
                dataGridRacun.Visibility = Visibility.Visible;
                dataGridRacun.ItemsSource = null;
                dataGridRacun.ItemsSource = stavke;  //stavke izabranog racuna
            }
            else
                dataGridRacun.Visibility = Visibility.Hidden;
            
        }
    }
}
