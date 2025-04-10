using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCashRegister.Models
{
    public class Osoba
    {
        public int OsobaId { get; set; }
        public string? Ime { get; set; } = string.Empty;
        public string? Prezime { get; set; } = string.Empty;
        public string? Jmbg { get; set; } = string.Empty;
        public string? Telefon { get; set; } = string.Empty;
        public string? Username {  get; set; } = string.Empty;
        public string? Sifra { get; set; } = string.Empty;
        public string? Uloga { get; set; } = string.Empty;

        public Osoba()
        {
        }
        public Osoba(int osobaId, string ime, string prezime, string jmbg, string telefon, string username, string sifra, string uloga)
        {
            OsobaId = osobaId;
            Ime = ime;
            Prezime = prezime;
            Jmbg = jmbg;
            Telefon = telefon;
            Username = username;
            Sifra = sifra;
            Uloga = uloga;
        }

        public Osoba(string ime, string prezime, string jmbg, string telefon, string username, string sifra, string uloga)
        {
            Ime = ime;
            Prezime = prezime;
            Jmbg = jmbg;
            Telefon = telefon;
            Username = username;
            Sifra = sifra;
            Uloga = uloga;
        }

        public override string? ToString()
        {
            return "Osoba:\nIme: " + Ime + "\tPrezime: " + Prezime + "\nJmbg: " + Jmbg + "\nTelefon: " + Telefon + "\nUloga: " + Uloga;
        }
    }
}
