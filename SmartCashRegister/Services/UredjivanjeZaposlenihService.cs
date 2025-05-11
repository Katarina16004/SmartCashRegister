using SmartCashRegister.Models;
using SmartCashRegister.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCashRegister.Services
{
    public class UredjivanjeZaposlenihService:IUredjivanjeZaposlenihService
    {
        private readonly IPristupBaziService _dbPristup;
        public UredjivanjeZaposlenihService(IPristupBaziService dbPristup)
        {
            _dbPristup = dbPristup;
        }
        public IEnumerable<Osoba> PrikaziSveZaposlene()
        {
            List<Osoba> svi = new List<Osoba>();
            string query = "SELECT * FROM Osoba";
            DataTable result = _dbPristup.ExecuteQuery(query);
            if (result.Rows.Count > 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    Osoba zaposleni = new Osoba
                    {
                        OsobaId = Convert.ToInt32(row["osoba_id"]),
                        Ime = row["ime"].ToString(),
                        Prezime = row["prezime"].ToString(),
                        Jmbg = row["jmbg"].ToString(),
                        Telefon = row["telefon"].ToString(),
                        Username = row["username"].ToString(),
                        Sifra= row["sifra"].ToString(),
                        Uloga= row["uloga"].ToString()
                    };
                    svi.Add(zaposleni);
                }
            }
            return svi;
        }
    }
}
