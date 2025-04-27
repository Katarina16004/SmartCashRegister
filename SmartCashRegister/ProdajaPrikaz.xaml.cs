using Microsoft.IdentityModel.Tokens;
using SmartCashRegister.Models;
using SmartCashRegister.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ProdajaPrikaz.xaml
    /// </summary>
    public partial class ProdajaPrikaz : UserControl
    {
        private readonly IKreiranjeRacunaService _kreiranjeRacunaService;
        private StavkaRacuna? selektovanaStavka;
        private Osoba prijavljeni;
        public ProdajaPrikaz(IKreiranjeRacunaService kreiranjeRacunaService,Osoba prijavljeni)
        {
            InitializeComponent();
            _kreiranjeRacunaService=kreiranjeRacunaService;
            this.prijavljeni = prijavljeni;
        }

        private void Button_DodajProizvod_Click(object sender, RoutedEventArgs e)
        {
            if(TextBox_BarKodProizvoda.Text.IsNullOrEmpty() || TextBox_KolicinaProizvoda.Text.IsNullOrEmpty())
            {
                MessageBox.Show("Morate uneti barkod i količinu");
                return;
            }
            if (_kreiranjeRacunaService.DodajProizvod(TextBox_BarKodProizvoda.Text,TextBox_KolicinaProizvoda.Text))
            {
                MessageBox.Show("Uspesno dodat proizvod");
                dataGridStavkeRacuna.ItemsSource = null;
                dataGridStavkeRacuna.ItemsSource= _kreiranjeRacunaService.GetStavkeRacuna();
                TextBox_BarKodProizvoda.Text = "";
                TextBox_KolicinaProizvoda.Text = "";
            }
        }

        private void Button_ObrisiProizvod_Click(object sender, RoutedEventArgs e)
        {
            if(selektovanaStavka!=null)
            {
                if (_kreiranjeRacunaService.ObrisiProizvod(selektovanaStavka))
                {
                    MessageBox.Show("Uspesno obrisan proizvod");
                    dataGridStavkeRacuna.ItemsSource = null;
                    dataGridStavkeRacuna.ItemsSource = _kreiranjeRacunaService.GetStavkeRacuna();
                }
            }
        }

        private void Button_KreirajRacun_Click(object sender, RoutedEventArgs e)
        {
            if(_kreiranjeRacunaService.KreirajRacun(prijavljeni.OsobaId))
            {
                dataGridStavkeRacuna.ItemsSource = null; //za novi racun
            }
        }

        private void dataGridStavkeRacuna_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selektovanaStavka = dataGridStavkeRacuna.SelectedItem as StavkaRacuna;
            if (selektovanaStavka != null)
            {
                Button_ObrisiProizvod.Visibility = Visibility.Visible;
            }
            else
                Button_ObrisiProizvod.Visibility = Visibility.Hidden;
        }

    }
}
