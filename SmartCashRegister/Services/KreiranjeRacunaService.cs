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
using System.Collections;

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
        public bool ObrisiProizvod(StavkaRacuna selektovanaStavka)
        {
            stavkeRacuna.Remove(selektovanaStavka);
            return true;
        }
        public bool KreirajRacun(int osobaId)
        {
            if (stavkeRacuna.Count == 0)
            {
                MessageBox.Show("Nema stavki na računu");
                return false;
            }

            decimal ukupnaCena = 0;
            foreach (StavkaRacuna s in stavkeRacuna)
            {
                ukupnaCena += s.Ukupno; // sabiranje ukupne cene
            }

            string query = @"
                INSERT INTO Racun (datum, cena, osoba_id, status) 
                VALUES (@Datum, @Cena, @OsobaId, @Status);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Datum", DateTime.Now),
                new SqlParameter("@Cena", ukupnaCena),
                new SqlParameter("@OsobaId", osobaId),
                new SqlParameter("@Status", "Aktivan")
            };

            DataTable result = _dbPristup.ExecuteQuery(query, parameters.ToArray());

            if (result.Rows.Count == 0)
            {
                MessageBox.Show("Greška pri kreiranju računa");
                return false;
            }

            int racunId = Convert.ToInt32(result.Rows[0][0]);

            foreach (var stavka in stavkeRacuna)
            {
                string stavkaQuery = @"
                    INSERT INTO StavkaRacuna (racun_id, proizvod_id, kolicina, cena) 
                    VALUES (@RacunId, @ProizvodId, @Kolicina, @Cena);";

                var stavkaParameters = new List<SqlParameter>
                {
                    new SqlParameter("@RacunId", racunId),
                    new SqlParameter("@ProizvodId", stavka.ProizvodId),
                    new SqlParameter("@Kolicina", stavka.Kolicina),
                    new SqlParameter("@Cena", stavka.Cena)
                };

                _dbPristup.ExecuteNonQuery(stavkaQuery, stavkaParameters.ToArray());
            }

            MessageBox.Show($"Račun uspešno kreiran! ID računa: {racunId}\nCena: {ukupnaCena}");

            stavkeRacuna.Clear();
            return true;
        }


    }
}
