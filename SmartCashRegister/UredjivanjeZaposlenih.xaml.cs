using SmartCashRegister.Services;
using SmartCashRegister.Services.Interfaces;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for UrediZaposlene.xaml
    /// </summary>
    public partial class UrediZaposlene : UserControl
    {
        private readonly IUredjivanjeZaposlenihService _uredjivanjeZaposlenihService;
        public UrediZaposlene(IUredjivanjeZaposlenihService uredjivanjeZaposlenihService)
        {
            _uredjivanjeZaposlenihService = uredjivanjeZaposlenihService;
            InitializeComponent();
        }

        private void Button_Pretrazi_Click(object sender, RoutedEventArgs e)
        {
            DataGrid_Zaposleni.Visibility = Visibility.Visible;
            if (Button_Pretrazi.Content.ToString() == "Pretrazi")
            {
                //DataGrid_Zaposleni.ItemsSource = _uredjivanjeZaposlenihService.PretraziZaposlenog(TextBox_Ime.Text, TextBox_Prezime.Text, TextBox_Username.Text);
            }
            else
            {
                DataGrid_Zaposleni.ItemsSource = _uredjivanjeZaposlenihService.PrikaziSveZaposlene();
            }
        }
    }
}
