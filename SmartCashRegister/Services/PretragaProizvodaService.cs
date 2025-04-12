using SmartCashRegister.Models;
using SmartCashRegister.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace SmartCashRegister.Services
{
    public class PretragaProizvodaService : IPretragaProizvodaService
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
            List<Proizvod> filtrirani = new List<Proizvod>();
            string query = "SELECT * FROM Proizvod p " +
                           "INNER JOIN Kategorija k ON p.kategorija_id = k.kategorija_id " +
                           "WHERE 1 = 1";
            List<SqlParameter> parameters = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(barKod))
            {
                query = query + " AND p.barkod = @BarKod";
                parameters.Add(new SqlParameter("@BarKod", barKod));
            }

            if (!string.IsNullOrEmpty(naziv))
            {
                query = query + " AND p.naziv LIKE @Naziv";
                parameters.Add(new SqlParameter("@Naziv", "%" + naziv + "%")); 
            }

            if (!string.IsNullOrEmpty(kategorija))
            {
                query = query + " AND k.naziv LIKE @Kategorija";
                parameters.Add(new SqlParameter("@Kategorija", "%" + kategorija + "%"));
            }

            DataTable result = _dbPristup.ExecuteQuery(query, parameters.ToArray());

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
                        KategorijaId = Convert.ToInt32(row["kategorija_id"]),
                    };
                    filtrirani.Add(proizvod);
                }
            }
            return filtrirani;
        }
    }
}