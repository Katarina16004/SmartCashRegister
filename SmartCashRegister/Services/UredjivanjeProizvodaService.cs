using SmartCashRegister.Models;
using SmartCashRegister.Services.Interfaces;
using System.Data;
using System.Windows;
using Microsoft.Data.SqlClient;

namespace SmartCashRegister.Services
{
    public class UredjivanjeProizvodaService:IUredjivanjeProizvodaService
    {
        private readonly IPristupBaziService _dbPristup;
        public UredjivanjeProizvodaService(IPristupBaziService dbPristup)
        {
            _dbPristup = dbPristup;
        }
        public bool DodajProizvod(Proizvod p)
        {
            if (DaLiPostojiBarkod(p.Barkod))
            {
                MessageBox.Show("Proizvod sa istim barkod-om već postoji");   
                return false;
            }

            string query = "INSERT INTO Proizvod (naziv, cena, kolicina, barkod, kategorija_id) " +
                   "VALUES (@naziv, @cena, @kolicina, @barkod, @kategorija_id)";

            SqlParameter[] parameters = {
                new SqlParameter("@naziv", p.Naziv),
                new SqlParameter("@cena", p.Cena),
                new SqlParameter("@kolicina", p.Kolicina),
                new SqlParameter("@barkod", p.Barkod),
                new SqlParameter("@kategorija_id", p.KategorijaId),

            };

            int rezultat = _dbPristup.ExecuteNonQuery(query,parameters);

            return rezultat > 0;
            
        }
        private bool DaLiPostojiBarkod(string? barkod)
        {
            string query = $"SELECT * FROM Proizvod WHERE barkod = '{barkod}'";
            DataTable dt = _dbPristup.ExecuteQuery(query);

            return dt.Rows.Count > 0;
        }
        public bool ObrisiProizvod(int proizvodId)
        {
            string query = $"DELETE FROM Proizvod WHERE proizvod_id = {proizvodId}";
            return _dbPristup.ExecuteNonQuery(query) > 0;
        }
    }
}
