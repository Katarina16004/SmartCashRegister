using SmartCashRegister.Models;
using SmartCashRegister.Services;
using System.Windows;

namespace SmartCashRegister
{
    /// <summary>
    /// Interaction logic for Meni.xaml
    /// </summary>
    public partial class Meni : Window
    {
        Osoba prijavljeni;
        private PristupBaziService dbPristup;
        private PretragaProizvodaService pretragaProizvodaService;
        private PrikazKategorijaService prikazKategorijaService;
        private PretragaRacunaService pretragaRacunaService;
        private KreiranjeRacunaService kreiranjeRacunaService;
        private UpravljanjeRacunomService upravljanjeRacunomService;
        public Meni(Osoba prijavljeni)
        {
            InitializeComponent();
            this.prijavljeni = prijavljeni;
            ImePrijavljenog.Text = prijavljeni.Ime;
            PrezimePrijavljenog.Text=prijavljeni.Prezime;

            if (prijavljeni.Uloga=="administrator")
            {
                ButtonUrediZaposlene.Visibility = Visibility.Visible;
                ButtonUrediKategorije.Visibility = Visibility.Visible;
                ButtonUrediProizvode.Visibility = Visibility.Visible;
            }

            dbPristup = new PristupBaziService();
            kreiranjeRacunaService = new KreiranjeRacunaService(dbPristup);
            MainContent.Content = new ProdajaPrikaz(kreiranjeRacunaService,prijavljeni);

            pretragaProizvodaService = new PretragaProizvodaService(dbPristup);
            prikazKategorijaService = new PrikazKategorijaService(dbPristup);
            pretragaRacunaService= new PretragaRacunaService(dbPristup);
            upravljanjeRacunomService = new UpravljanjeRacunomService(dbPristup, pretragaRacunaService);
        }

        private void ButtonProdaja_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ProdajaPrikaz(kreiranjeRacunaService, prijavljeni);
        }

        private void ButtonRacuni_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new RacuniPrikaz(pretragaRacunaService,prijavljeni,upravljanjeRacunomService);
        }

        private void ButtonOdjava_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void ButtonProizvodi_Click(object sender, RoutedEventArgs e)
        {
            if (prijavljeni.Uloga == "administrator")
                MainContent.Content = new UredjivanjeProizvoda(pretragaProizvodaService, prikazKategorijaService);
            else
                MainContent.Content = new ProizvodiPrikaz(pretragaProizvodaService,prikazKategorijaService);
        }
    }
}
