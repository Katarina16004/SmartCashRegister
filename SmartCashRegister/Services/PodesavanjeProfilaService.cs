using Microsoft.Data.SqlClient;
using SmartCashRegister.Models;

namespace SmartCashRegister.Services.Interfaces
{
    public class PodesavanjeProfilaService:IPodesavanjeProfilaService
    {
        private readonly IPristupBaziService _dbPristup;

        public PodesavanjeProfilaService(IPristupBaziService dbPristup)
        {
            _dbPristup = dbPristup;
        }

        public bool SacuvajPromeneAkoPostoje(Osoba osoba, string novoIme, string novoPrezime, string noviTelefon, string noviUsername, string novaLozinka)
        {
            var uslovi = new List<string>();
            var parameters = new List<SqlParameter>();

            if (osoba.Ime != novoIme)
            {
                uslovi.Add("ime = @ime");
                parameters.Add(new SqlParameter("@ime", novoIme));
            }
            if (osoba.Prezime != novoPrezime)
            {
                uslovi.Add("prezime = @prezime");
                parameters.Add(new SqlParameter("@prezime", novoPrezime));
            }
            if (osoba.Telefon != noviTelefon)
            {
                uslovi.Add("telefon = @telefon");
                parameters.Add(new SqlParameter("@telefon", noviTelefon));
            }
            if (osoba.Username != noviUsername)
            {
                uslovi.Add("username = @username");
                parameters.Add(new SqlParameter("@username", noviUsername));
            }
            if (osoba.Sifra != novaLozinka)
            {
                uslovi.Add("sifra = @sifra");
                parameters.Add(new SqlParameter("@sifra", novaLozinka));
            }

            if (uslovi.Count == 0)
            {
                return false;
            }

            string query = $"UPDATE Osoba SET {string.Join(", ", uslovi)} WHERE osoba_id = @osoba_id";
            parameters.Add(new SqlParameter("@osoba_id", osoba.OsobaId));

            int result = _dbPristup.ExecuteNonQuery(query, parameters.ToArray());

            if (result > 0)
            {
                osoba.Ime = novoIme;
                osoba.Prezime = novoPrezime;
                osoba.Telefon = noviTelefon;
                osoba.Username = noviUsername;
                osoba.Sifra = novaLozinka;

                return true;
            }

            return false;
        }
    }
}
