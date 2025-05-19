using SmartCashRegister.Models;
using SmartCashRegister.Services.Interfaces;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Runtime.Intrinsics.X86;
using System.Windows;

namespace SmartCashRegister.Services
{
    public class UredjivanjeKategorijaService:IUredjivanjeKategorijaService
    {
        private readonly IPristupBaziService _dbPristup;
        public UredjivanjeKategorijaService(IPristupBaziService dbPristup)
        {
            _dbPristup = dbPristup;
        }
        public IEnumerable<Kategorija> PrikaziSveKategorije()
        {
            List<Kategorija> sve = new List<Kategorija>();
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
                    sve.Add(kat);
                }
            }
            return sve;
        }
        public IEnumerable<Kategorija> PretraziKategoriju(string naziv)
        {
            List<Kategorija> filtrirane = new List<Kategorija>();
            string query = "SELECT * FROM Kategorija " +
                           "WHERE naziv LIKE @naziv";

            SqlParameter[] parameters =
            {
                new SqlParameter("@naziv",  "%" + naziv + "%")
            };

            DataTable result = _dbPristup.ExecuteQuery(query, parameters.ToArray());

            if (result.Rows.Count > 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    Kategorija kat = new Kategorija
                    {
                        KategorijaId = Convert.ToInt32(row["kategorija_id"]),
                        Naziv = row["naziv"].ToString()
                    };
                    filtrirane.Add(kat);
                }
            }
            return filtrirane;
        }
        public bool DodajKategoriju(Kategorija k)
        {
            if (DaLiPostojiNaziv(k.Naziv))
            {
                MessageBox.Show("Kategorija sa istim nazivom već postoji");
                return false;
            }

            string query = $"INSERT INTO Kategorija (naziv) " +
                           $"VALUES ('{k.Naziv}')";

            int rezultat = _dbPristup.ExecuteNonQuery(query);

            return rezultat > 0;
        }
        private bool DaLiPostojiNaziv(string? naziv)
        {
            string query = $"SELECT * FROM Kategorija WHERE naziv = '{naziv}'";
            DataTable dt = _dbPristup.ExecuteQuery(query);

            return dt.Rows.Count > 0;
        }
    }
}
