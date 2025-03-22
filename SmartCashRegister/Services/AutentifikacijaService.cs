using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace SmartCashRegister.Services
{
    public class AutentifikacijaService:IAutentifikacijaService
    {
        private readonly IPristupBaziService _dbPristup;

        public AutentifikacijaService(IPristupBaziService dbPristup)
        {
            _dbPristup = dbPristup;
        }

        public bool UspesnaPrijava(string username, string password)
        {
            string query = "SELECT ime FROM Osoba WHERE username = @Username AND sifra = @Password";
            SqlParameter[] parameters = {
                new SqlParameter("@Username", username),
                new SqlParameter("@Password", password)
            };

            DataTable result = _dbPristup.ExecuteQuery(query, parameters);

            return result.Rows.Count > 0;
        }
    }
}
