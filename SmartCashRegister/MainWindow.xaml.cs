using SmartCashRegister.Services;
using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AutentifikacijaService _autentifikacija;
        public MainWindow()
        {
            InitializeComponent();
            UsernameTextBox.Focus();
            IPristupBaziService dbPristup = new PristupBaziService();
            _autentifikacija = new AutentifikacijaService(dbPristup);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Molimo Vas da popunite sva polja");
                return;
            }

            bool isAuthenticated = _autentifikacija.UspesnaPrijava(username, password);

            if (isAuthenticated)
            {
                MessageBox.Show("Prijavljeni ste!");
            }
            else
            {
                MessageBox.Show("Neispravno korisničko ime ili sifra");
            }
        }
    }
}