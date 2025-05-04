

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
            string query = $"UPDATE Osoba SET ime='{novoIme}' WHERE osoba_id = {prijavljeniId} AND ime NOT LIKE '{novoIme}'";
            int affected = _dbPristup.ExecuteNonQuery(query);

            if (affected == 0)
            {
                MessageBox.Show("Greška pri izmeni imena");
                return false;
            }
            MessageBox.Show("Uspešno ste promenili ime");
            return true;
        }
        public bool PromeniPrezime(int prijavljeniId, string novoPrezime)
        {
            return true;
        }
        public bool PromeniTelefon(int prijavljeniId, string noviTelefon)
        {
            return true;
        }
        public bool PromeniUsername(int prijavljeniId, string noviUsername)
        {
            return true;
        }
        public bool PromeniLozinku(int prijavljeniId, string novaLozinka)
        {
            return true;
        }
    }
}
