using SmartCashRegister.Models;
using SmartCashRegister.Services;
using SmartCashRegister.Services.Interfaces;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace SmartCashRegister
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AutentifikacijaService _autentifikacija;
        bool vidljivostLozinke = false;
        Osoba? prijavljeni;
        public MainWindow()
        {
            InitializeComponent();
            IPristupBaziService dbPristup = new PristupBaziService();
            _autentifikacija = new AutentifikacijaService(dbPristup);

            PasswordBox.PasswordChanged += PasswordBox_PasswordChanged;
            EyeIcon.Visibility = Visibility.Collapsed;
            UsernameTextBox.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password;
            if (PasswordBox.Visibility == Visibility.Visible)
                password = PasswordBox.Password;
            else
                password = PasswordTextBox.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Molimo Vas da popunite sva polja");
                return;
            }
            prijavljeni = _autentifikacija.UspesnaPrijava(username, password);
            if (prijavljeni!=null)
            { 
                Meni meniWindow=new Meni(prijavljeni);
                this.Close();

                meniWindow.ShowDialog();
                
                //MessageBox.Show("Prijavljeni ste!");
            }
            else
            {
                MessageBox.Show("Neispravno korisničko ime ili sifra");
            }
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
                PasswordBox.Password = PasswordTextBox.Text;

                EyeIcon.Source = new BitmapImage(new System.Uri("Resources/otvoreno_oko.png", System.UriKind.Relative));
            }
            else
            {
                PasswordTextBox.Visibility = Visibility.Visible;
                PasswordBox.Visibility = Visibility.Collapsed;
                PasswordTextBox.Text = PasswordBox.Password;

                EyeIcon.Source = new BitmapImage(new System.Uri("Resources/zatvoreno_oko.png", System.UriKind.Relative));
            }

            vidljivostLozinke = !vidljivostLozinke;
        }
    }
}