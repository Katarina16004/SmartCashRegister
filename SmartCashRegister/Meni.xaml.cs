using SmartCashRegister.Models;
using SmartCashRegister.Services;
using SmartCashRegister.Services.Interfaces;
using System.Windows;
using System.Windows.Controls;

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
        private PodesavanjeProfilaService podesavanjeProfilaService;
        private UredjivanjeZaposlenihService uredjivanjeZaposlenihService;
        private UredjivanjeProizvodaService uredjivanjeProizvodaService;
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
            AktivanButton(ButtonProdaja);
            MainContent.Content = new ProdajaPrikaz(kreiranjeRacunaService,prijavljeni);


            pretragaProizvodaService = new PretragaProizvodaService(dbPristup);
            prikazKategorijaService = new PrikazKategorijaService(dbPristup);
            pretragaRacunaService= new PretragaRacunaService(dbPristup);
            upravljanjeRacunomService = new UpravljanjeRacunomService(dbPristup, pretragaRacunaService);
            podesavanjeProfilaService=new PodesavanjeProfilaService(dbPristup);
            uredjivanjeZaposlenihService=new UredjivanjeZaposlenihService(dbPristup);
            uredjivanjeProizvodaService = new UredjivanjeProizvodaService(dbPristup);
        }

        private void ButtonProdaja_Click(object sender, RoutedEventArgs e)
        {
            AktivanButton(ButtonProdaja);
            MainContent.Content = new ProdajaPrikaz(kreiranjeRacunaService, prijavljeni);
        }

        private void ButtonRacuni_Click(object sender, RoutedEventArgs e)
        {
            AktivanButton(ButtonRacuni);
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
            AktivanButton(ButtonProizvodi);
            MainContent.Content = new ProizvodiPrikaz(pretragaProizvodaService,prikazKategorijaService);
        }
        private void AktivanButton(Button trenutni)
        {
            Button[] buttons = { ButtonProdaja, ButtonRacuni, ButtonProizvodi, ButtonPodesavanjaProfila, ButtonUrediZaposlene, ButtonUrediProizvode, ButtonUrediKategorije };
            foreach (var button in buttons)
            {
                if (button == trenutni)
                    button.Style = (Style)FindResource("ActiveModernButton");
                else
                    button.Style = (Style)FindResource("ModernButton");
            }
        }

        private void ButtonPodesavanjaProfila_Click(object sender, RoutedEventArgs e)
        {
            AktivanButton(ButtonPodesavanjaProfila);
            MainContent.Content = new PodesavanjeProfila(prijavljeni,dbPristup,podesavanjeProfilaService);
        }

        private void ButtonUrediZaposlene_Click(object sender, RoutedEventArgs e)
        {
            AktivanButton(ButtonUrediZaposlene);
            MainContent.Content = new UrediZaposlene(uredjivanjeZaposlenihService,dbPristup);
        }

        private void ButtonUrediProizvode_Click(object sender, RoutedEventArgs e)
        {
            AktivanButton(ButtonUrediProizvode);
            MainContent.Content = new UredjivanjeProizvoda(pretragaProizvodaService,dbPristup, uredjivanjeProizvodaService);
        }

        private void ButtonUrediKategorije_Click(object sender, RoutedEventArgs e)
        {
            AktivanButton(ButtonUrediKategorije);
            MainContent.Content = new UredjivanjeKategorija();
        }
    }
}
