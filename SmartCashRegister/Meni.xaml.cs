using SmartCashRegister.Models;
using System.Windows;

namespace SmartCashRegister
{
    /// <summary>
    /// Interaction logic for Meni.xaml
    /// </summary>
    public partial class Meni : Window
    {
        Osoba prijavljeni;
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
            MainContent.Content = new ProdajaPrikaz();
        }

        private void ButtonProdaja_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ProdajaPrikaz();
        }

        private void ButtonRacuni_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new RacuniPrikaz();
        }

        private void ButtonOdjava_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.ShowDialog();
            this.Close();
        }

        private void ButtonProizvodi_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ProizvodiPrikaz();
        }
    }
}
