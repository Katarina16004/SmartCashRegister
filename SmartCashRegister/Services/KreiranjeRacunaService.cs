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
using Accessibility;

namespace SmartCashRegister.Services
{
    public class KreiranjeRacunaService:IKreiranjeRacunaService
    {
        private readonly IPristupBaziService _dbPristup;
        private readonly IUpravljanjeRacunomService _upravljanjeRacunomService;
        private List<StavkaRacuna> stavkeRacuna = new List<StavkaRacuna>();
        public KreiranjeRacunaService(IPristupBaziService dbPristup, IUpravljanjeRacunomService upravljanjeRacunomService)
        {
            _dbPristup = dbPristup;
            _upravljanjeRacunomService = upravljanjeRacunomService;
        }
        public bool DodajProizvod(string barkod, string kol)
        {
            int trazenaKolicina = Convert.ToInt32(kol);

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
                    int vecDodato = 0;
                    foreach (var stavka in stavkeRacuna)
                    {
                        if (stavka.ProizvodId == Convert.ToInt32(row["proizvod_id"]))
                        {
                            vecDodato += stavka.Kolicina;
                        }
                    }

                    int dostupnoZaDodavanje = kolicinaNaSkladistu - vecDodato;

                    if (dostupnoZaDodavanje == 0)
                    {
                        MessageBox.Show("Nema više proizvoda na skladištu");
                        return false;
                    }
                    if (trazenaKolicina > dostupnoZaDodavanje)
                    {
                        MessageBox.Show($"Najviše što možete uzeti je {dostupnoZaDodavanje}");
                        return false;
                    }

                    StavkaRacuna novaStavka = new StavkaRacuna
                    {
                        ProizvodId = Convert.ToInt32(row["proizvod_id"]),
                        Naziv = row["naziv"].ToString(),
                        Cena = Convert.ToDecimal(row["cena"]),
                        Kolicina = trazenaKolicina
                    };
                    stavkeRacuna.Add(novaStavka);
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
        public bool SetStavkeRacuna(List<StavkaRacuna> stavke)
        {
            stavkeRacuna = stavke;
            return true;
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
            var potvrda = MessageBox.Show($"Ukupna cena računa: {ukupnaCena}", "Potvrda", MessageBoxButton.YesNo); //potvrdi se ukoliko je racun placen
            if (potvrda != MessageBoxResult.Yes)
            {
                stavkeRacuna.Clear();
                return false;
            }

            string query = @"
                INSERT INTO Racun (datum, cena, osoba_id, status) 
                VALUES (@Datum, @Cena, @OsobaId, @Status);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            DateTime vremeKreiranja = DateTime.Now;
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Datum", vremeKreiranja),
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

                //azuriramo kolicine u bazi
                string updateQuery = @"
                    UPDATE Proizvod 
                    SET kolicina = kolicina - @Kolicina 
                    WHERE proizvod_id = @ProizvodId;";

                var updateParams = new List<SqlParameter>
                {
                    new SqlParameter("@Kolicina", stavka.Kolicina),
                    new SqlParameter("@ProizvodId", stavka.ProizvodId)
                };

                _dbPristup.ExecuteNonQuery(updateQuery, updateParams.ToArray());
            }

            var racun = new Racun
            {
                RacunId = racunId,
                Datum = vremeKreiranja,
                Cena = ukupnaCena,
                OsobaId = osobaId,
                Status = "Aktivan"
            };

            _upravljanjeRacunomService.IzveziUPDF(racun);
            stavkeRacuna.Clear();
            return true;
        }


    }
}
