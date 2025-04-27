using SmartCashRegister.Models;
using SmartCashRegister.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;

namespace SmartCashRegister.Services
{
    public class KreiranjeRacunaService:IKreiranjeRacunaService
    {
        private readonly IPristupBaziService _dbPristup;
        private List<StavkaRacuna> stavkeRacuna = new List<StavkaRacuna>();
        public KreiranjeRacunaService(IPristupBaziService dbPristup)
        {
            _dbPristup = dbPristup;
        }
        public bool DodajProizvod(string barkod, string kol)
        {
            string query = "SELECT * FROM Proizvod WHERE barkod = @Barkod";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Barkod", barkod)
            };
            DataTable result = _dbPristup.ExecuteQuery(query, parameters.ToArray());
            if (result.Rows.Count > 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    int kolicinaNaSkladistu = Convert.ToInt32(row["kolicina"]);
                    if (kolicinaNaSkladistu < Convert.ToInt32(kol))
                    {
                        MessageBox.Show("Nema dovoljno proizvoda na skladištu");
                        return false;
                    }
                    StavkaRacuna stavka = new StavkaRacuna
                    {
                        ProizvodId = Convert.ToInt32(row["proizvod_id"]),
                        Naziv = row["naziv"].ToString(),
                        Cena = Convert.ToDecimal(row["cena"]),
                        Kolicina = Convert.ToInt32(kol)
                    };
                    stavkeRacuna.Add(stavka);
                }
            }
            else
            {
                MessageBox.Show("Proizvod sa takvim barkodom ne postoji");
                return false;
            }
            return true;
        }
        public List<StavkaRacuna> GetStavkeRacuna()
        {
            return stavkeRacuna;
        }
    }
}
