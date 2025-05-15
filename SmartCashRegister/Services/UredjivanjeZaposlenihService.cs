using SmartCashRegister.Models;
using SmartCashRegister.Services.Interfaces;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows.Controls;
using System.Windows;

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
        public IEnumerable<Osoba> PretraziZaposlenog(string ime = "", string prezime = "", string username = "")
        {
            List<Osoba> filtrirani = new List<Osoba>();
            string query = "SELECT * FROM Osoba " +
                           "WHERE 1 = 1";
            List<SqlParameter> parameters = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(ime))
            {
                query = query + " AND ime LIKE @Ime";
                parameters.Add(new SqlParameter("@Ime", "%" + ime + "%"));
            }

            if (!string.IsNullOrEmpty(prezime))
            {
                query = query + " AND prezime LIKE @Prezime";
                parameters.Add(new SqlParameter("@Prezime", "%" + prezime + "%"));
            }

            if (!string.IsNullOrEmpty(username))
            {
                query = query + " AND username LIKE @Username";
                parameters.Add(new SqlParameter("@Username", "%" + username + "%"));
            }

            DataTable result = _dbPristup.ExecuteQuery(query, parameters.ToArray());

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
                        Sifra = row["sifra"].ToString(),
                        Uloga = row["uloga"].ToString()
                    };
                    filtrirani.Add(zaposleni);
                }
            }
            return filtrirani;
        }
        public bool DodajZaposlenog(Osoba o)
        {
            if (DaLiPostojiJmbgIliUsername(o.Jmbg, o.Username))
            {
                MessageBox.Show("Zaposleni sa istim JMBG-om ili korisničkim imenom već postoji");
                return false;
            }

            string query = $"INSERT INTO Osoba (ime, prezime, jmbg, telefon, username, sifra, uloga) " +
                           $"VALUES ('{o.Ime}', '{o.Prezime}', '{o.Jmbg}', '{o.Telefon}', '{o.Username}', '{o.Sifra}', '{o.Uloga}')";

            int rezultat = _dbPristup.ExecuteNonQuery(query);

            return rezultat > 0;
        }
        private bool DaLiPostojiJmbgIliUsername(string? jmbg, string? username)
        {
            string query = $"SELECT * FROM Osoba WHERE jmbg = '{jmbg}' OR username = '{username}'";
            DataTable dt = _dbPristup.ExecuteQuery(query);

            return dt.Rows.Count > 0;
        }
        private bool DaLiPostojiJmbgIliUsernameOsimOvog(string? jmbg, string? username, int osobaId)
        {
            string query = $"SELECT * FROM Osoba WHERE (jmbg = '{jmbg}' OR username = '{username}') AND osoba_id!={osobaId}";
            DataTable dt = _dbPristup.ExecuteQuery(query);

            return dt.Rows.Count > 0;
        }
        public bool ObrisiZaposlenog(int osobaId)
        {
            string query = $"DELETE FROM Osoba WHERE osoba_id = {osobaId}";
            return _dbPristup.ExecuteNonQuery(query) > 0;
        }
        public bool IzmeniZaposlenog(Osoba o)
        {
            if (DaLiPostojiJmbgIliUsernameOsimOvog(o.Jmbg, o.Username,o.OsobaId))
            {
                MessageBox.Show("Zaposleni sa istim JMBG-om ili korisničkim imenom već postoji");
                return false;
            }
            string query = $@"
                UPDATE Osoba SET
                    ime = '{o.Ime}',
                    prezime = '{o.Prezime}',
                    jmbg = '{o.Jmbg}',
                    telefon = '{o.Telefon}',
                    username = '{o.Username}',
                    sifra = '{o.Sifra}',
                    uloga = '{o.Uloga}'
                WHERE osoba_id = {o.OsobaId}";

            return _dbPristup.ExecuteNonQuery(query) > 0;
        }
    }
}
