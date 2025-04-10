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
    public class PretragaProizvodaService:IPretragaProizvodaService
    {
        private readonly IPristupBaziService _dbPristup;
        public PretragaProizvodaService(IPristupBaziService dbPristup)
        {
            _dbPristup = dbPristup;
        }

        public IEnumerable<Proizvod> PrikaziSveProizvode()
        {
            List<Proizvod> svi = new List<Proizvod>();
            string query = "SELECT * FROM Proizvod";
            DataTable result = _dbPristup.ExecuteQuery(query);
            if (result.Rows.Count > 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    Proizvod proizvod = new Proizvod
                    {
                        ProizvodId = Convert.ToInt32(row["proizvod_id"]),
                        Naziv = row["naziv"].ToString(),
                        Cena = Convert.ToDecimal(row["cena"]),
                        Kolicina = Convert.ToInt32(row["kolicina"]),
                        Barkod = row["barkod"].ToString(),
                        KategorijaId = Convert.ToInt32(row["kategorija_id"])
                    };
                    svi.Add(proizvod);
                }
            }
            return svi;
        }
        public IEnumerable<Proizvod> PretraziProizvod(string barKod = "", string naziv = "", string kategorija = "")
        {
            List<Proizvod> filtrirani=new List<Proizvod>();
            return filtrirani;
        }
    }
}
