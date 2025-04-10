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
    public class PrikazKategorijaService:IPrikazKategorijaService
    {
        private readonly IPristupBaziService _dbPristup;
        public PrikazKategorijaService(IPristupBaziService dbPristup)
        {
            _dbPristup = dbPristup;
        }

        public IEnumerable<Kategorija> PrikaziSve()
        {
            List<Kategorija> kategorije = new List<Kategorija>();
            string query = "SELECT * FROM Kategorija";
            DataTable result = _dbPristup.ExecuteQuery(query);
            if (result.Rows.Count > 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    Kategorija kat = new Kategorija
                    {
                        KategorijaId = Convert.ToInt32(row["kategorija_id"]),
                        Naziv = row["naziv"].ToString()
                    };
                    kategorije.Add(kat);
                }
            }
            return kategorije;
        }
    }
}
