﻿using iTextSharp.text.pdf;
using SmartCashRegister.Models;
using SmartCashRegister.Services;
using SmartCashRegister.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
        private readonly IUredjivanjeProizvodaService _uredjivanjeProizvodaService;
        public UredjivanjeProizvoda(IPretragaProizvodaService pretragaProizvodaService, IPristupBaziService dbPristup, IUredjivanjeProizvodaService uredjivanjeProizvodaService)
        {
            _pretragaProizvodaService = pretragaProizvodaService;
            _uredjivanjeProizvodaService = uredjivanjeProizvodaService;
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
                decimal cena = Convert.ToDecimal(row["cena"]);
                Input_Cena.Text = cena.ToString(CultureInfo.InvariantCulture);
                //Input_Cena.Text = row["cena"].ToString();
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

        private void Button_Dodaj_Click(object sender, RoutedEventArgs e)
        {
            if(ProveriPolja())
            {
                if (!decimal.TryParse(Input_Cena.Text.Replace('.', ','), out decimal cena))
                {
                    MessageBox.Show("Cena mora biti realan broj", "Neispravan format cene", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(Input_Kolicina.Text, out int kolicina) || kolicina<1)
                {
                    MessageBox.Show("Količina mora biti ceo, pozitivan broj", "Neispravan format količine", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                Proizvod noviProizvod = new Proizvod
                {
                    Naziv = Input_Naziv.Text,
                    Cena = cena,
                    Kolicina = kolicina,
                    Barkod = Input_Barkod.Text,
                    KategorijaId = (int)ComboBox_Kategorija.SelectedValue
                };

                bool uspeh = _uredjivanjeProizvodaService.DodajProizvod(noviProizvod);

                if (uspeh)
                {
                    MessageBox.Show("Proizvod je uspešno dodat");
                    OsveziDataGrid();
                    OcistiPolja();
                }
            }
        }
        private bool ProveriPolja()
        {
            if (Input_Naziv.Text == "" ||
                Input_Cena.Text == "" ||
                Input_Kolicina.Text == "" ||
                Input_Barkod.Text == "" ||
                ComboBox_Kategorija.SelectedValue == null)
            {
                MessageBox.Show("Morate popuniti SVA polja");
                return false;
            }
            return true;
        }
        private void OcistiPolja()
        {
            Input_Naziv.Clear();
            Input_Cena.Clear();
            Input_Kolicina.Clear();
            Input_Barkod.Clear();
            ComboBox_Kategorija.SelectedIndex = -1;
            dataGridProizvodi.SelectedItem = null;
            Button_Izmeni.Visibility = Visibility.Hidden;
            Button_Obrisi.Visibility = Visibility.Hidden;
        }
        private void OsveziDataGrid()
        {
            dataGridProizvodi.ItemsSource = null;
            dataGridProizvodi.ItemsSource = _pretragaProizvodaService.PrikaziSveProizvode();
        }

        private void Button_Obrisi_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridProizvodi.SelectedItem is Proizvod selektovaniProizvod)
            {
                var potvrda = MessageBox.Show("Da li ste sigurni da želite da obrišete proizvod?", "Potvrda", MessageBoxButton.YesNo);
                if (potvrda != MessageBoxResult.Yes) return;

                bool uspeh = _uredjivanjeProizvodaService.ObrisiProizvod(selektovaniProizvod.ProizvodId);

                if (uspeh)
                {
                    MessageBox.Show("Proizvod je obrisan");
                    OsveziDataGrid();
                    OcistiPolja();
                }
            }
        }

        private void Button_Izmeni_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridProizvodi.SelectedItem is Proizvod selektovaniProizvod)
            {
                if (!decimal.TryParse(Input_Cena.Text.Replace('.',','), out decimal cena))
                {
                    MessageBox.Show("Cena mora biti realan broj", "Neispravan format cene", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(Input_Kolicina.Text, out int kolicina) || kolicina < 1)
                {
                    MessageBox.Show("Količina mora biti ceo, pozitivan broj", "Neispravan format količine", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                bool nestoSePromenilo =
                    selektovaniProizvod.Naziv != Input_Naziv.Text ||
                    selektovaniProizvod.Cena != cena ||
                    selektovaniProizvod.Kolicina != kolicina ||
                    selektovaniProizvod.Barkod != Input_Barkod.Text ||
                    selektovaniProizvod.KategorijaId != (int)ComboBox_Kategorija.SelectedValue;

                if (!nestoSePromenilo)
                {
                    MessageBox.Show("Nema promena za čuvanje");
                    return;
                }

                selektovaniProizvod.Naziv = Input_Naziv.Text;
                selektovaniProizvod.Cena = cena;
                selektovaniProizvod.Kolicina=kolicina;
                selektovaniProizvod.Barkod= Input_Barkod.Text;
                selektovaniProizvod.KategorijaId = (int)ComboBox_Kategorija.SelectedValue;

                if (ProveriPolja())
                {

                    bool uspeh = _uredjivanjeProizvodaService.IzmeniProizvod(selektovaniProizvod);

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
