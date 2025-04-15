using SmartCashRegister.Models;
using SmartCashRegister.Services.Interfaces;
using System.Data;
using Microsoft.Data.SqlClient;

namespace SmartCashRegister.Services
{
    class PretragaRacunaService : IPretragaRacunaService
    {
        private readonly IPristupBaziService _dbPristup;

        public PretragaRacunaService(IPristupBaziService dbPristup)
        {
            _dbPristup = dbPristup;
        }
        public IEnumerable<Racun> PrikaziSveRacune()
        {
            List<Racun> svi = new List<Racun>();
            string query = "SELECT * FROM Racun";
            DataTable result = _dbPristup.ExecuteQuery(query);
            if (result.Rows.Count > 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    Racun racun = new Racun
                    {
                        RacunId = Convert.ToInt32(row["racun_id"]),
                        Datum = Convert.ToDateTime(row["datum"]),
                        Cena = Convert.ToDecimal(row["cena"]),
                        OsobaId = Convert.ToInt32(row["osoba_id"])
                    };
                    svi.Add(racun);
                }
            }
            return svi;
        }
        public IEnumerable<Racun> PretraziRacun(string idRacuna = "", string username = "", DateTime? datum = null)
        {
            List<Racun> filtrirani = new List<Racun>();
            string query = "SELECT * FROM Racun r " +
                           "INNER JOIN Osoba o ON r.osoba_id=o.osoba_id " +
                           "WHERE 1 = 1";
            List<SqlParameter> parameters = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(idRacuna))
            {
                query = query + " AND r.racun_id = @RacunId";
                parameters.Add(new SqlParameter("@RacunId", idRacuna));
            }

            if (!string.IsNullOrEmpty(username))
            {
                query = query + " AND o.username LIKE @Username";
                parameters.Add(new SqlParameter("@Username", "%" + username + "%"));
            }

            if (datum.HasValue)
            {
                query += " AND CAST(r.datum AS DATE) = @Datum";
                parameters.Add(new SqlParameter("@Datum", datum.Value.Date));
            }

            DataTable result = _dbPristup.ExecuteQuery(query, parameters.ToArray());

            if (result.Rows.Count > 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    Racun racun = new Racun
                    {
                        RacunId = Convert.ToInt32(row["racun_id"]),
                        Datum = Convert.ToDateTime(row["datum"]),
                        Cena = Convert.ToDecimal(row["cena"]),
                        OsobaId = Convert.ToInt32(row["osoba_id"])
                    };
                    filtrirani.Add(racun);
                }
            }
            return filtrirani;
        }
        public IEnumerable<StavkaRacuna> PrikaziStavkeRacuna(int racunId)
        {
            string query = "SELECT s.stavka_id, s.proizvod_id, p.naziv, s.kolicina, s.cena " +
                "FROM StavkaRacuna s "+
                "JOIN Proizvod p ON s.proizvod_id = p.proizvod_id WHERE s.racun_id = @racunId";

            SqlParameter[] parameters = {
                new SqlParameter("@racunId", racunId)
            };

            DataTable dt = _dbPristup.ExecuteQuery(query, parameters.ToArray());

            List<StavkaRacuna> stavke = new List<StavkaRacuna>();

            foreach (DataRow row in dt.Rows)
            {
                stavke.Add(new StavkaRacuna
                {
                    ProizvodId = Convert.ToInt32(row["proizvod_id"]),
                    Naziv = row["naziv"].ToString(),
                    Kolicina = Convert.ToInt32(row["kolicina"]),
                    Cena = Convert.ToDecimal(row["cena"])
                });
            }
            return stavke;
        }
    }
}
