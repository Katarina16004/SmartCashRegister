using System.Data;
using Microsoft.Data.SqlClient;
using SmartCashRegister.Models;

namespace SmartCashRegister.Services
{
    public class AutentifikacijaService:IAutentifikacijaService
    {
        private readonly IPristupBaziService _dbPristup;

        public AutentifikacijaService(IPristupBaziService dbPristup)
        {
            _dbPristup = dbPristup;
        }

        public Osoba? UspesnaPrijava(string username, string password)
        {
            string query = "SELECT * FROM Osoba WHERE username = @Username AND sifra = @Password";
            SqlParameter[] parameters = {
                new SqlParameter("@Username", username),
                new SqlParameter("@Password", password)
            };

            DataTable result = _dbPristup.ExecuteQuery(query, parameters);
            if (result.Rows.Count > 0)
            {
                DataRow row = result.Rows[0];
                Osoba osoba = new Osoba
                {
                    OsobaId = Convert.ToInt32(row["osoba_id"]),
                    Ime = row["ime"].ToString(),
                    Prezime = row["prezime"].ToString(),
                    Jmbg = row["jmbg"].ToString(),
                    Telefon = row["telefon"].ToString(),
                    Username = row["username"].ToString(),
                    Sifra = row["sifra"].ToString(),
                    Uloga = row["uloga"].ToString()
                };

                return osoba;
            }
            else
            {
                return null;
            }
        }
    }
}
