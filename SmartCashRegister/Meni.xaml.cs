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
            MainContent.Content = new ProdajaPrikaz(kreiranjeRacunaService);

            pretragaProizvodaService = new PretragaProizvodaService(dbPristup);
            prikazKategorijaService = new PrikazKategorijaService(dbPristup);
            pretragaRacunaService= new PretragaRacunaService(dbPristup);
        }

        private void ButtonProdaja_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ProdajaPrikaz(kreiranjeRacunaService);
        }

        private void ButtonRacuni_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new RacuniPrikaz(pretragaRacunaService,prijavljeni);
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
