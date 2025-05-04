

using System.Windows;

namespace SmartCashRegister.Services.Interfaces
{
    public class PodesavanjeProfilaService:IPodesavanjeProfilaService
    {
        private readonly IPristupBaziService _dbPristup;
        public PodesavanjeProfilaService(IPristupBaziService dbPristup)
        {
            _dbPristup = dbPristup;
        }
        public bool PromeniIme(int prijavljeniId, string novoIme)
        {
            return IzmeniPolje("ime", novoIme, prijavljeniId, "imena");
        }

        public bool PromeniPrezime(int prijavljeniId, string novoPrezime)
        {
            return IzmeniPolje("prezime", novoPrezime, prijavljeniId, "prezimena");
        }

        public bool PromeniTelefon(int prijavljeniId, string noviTelefon)
        {
            return IzmeniPolje("telefon", noviTelefon, prijavljeniId, "telefona");
        }

        public bool PromeniUsername(int prijavljeniId, string noviUsername)
        {
            return IzmeniPolje("username", noviUsername, prijavljeniId, "username-a");
        }

        public bool PromeniLozinku(int prijavljeniId, string novaLozinka)
        {
            return IzmeniPolje("sifra", novaLozinka, prijavljeniId, "lozinke");
        }

        private bool IzmeniPolje(string nazivKolone, string novaVrednost, int osobaId, string nazivPoljaZaPrikaz)
        {
            string query = $"UPDATE Osoba SET {nazivKolone} = '{novaVrednost}' WHERE osoba_id = {osobaId}";

            int rezultat = _dbPristup.ExecuteNonQuery(query);

            if (rezultat == 0)
            {
                MessageBox.Show("Greška pri izmeni " + nazivPoljaZaPrikaz);
                return false;
            }
            return true;
        }
    }
}
