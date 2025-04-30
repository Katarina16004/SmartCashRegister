using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCashRegister.Models
{
    public class Racun
    {
        public int RacunId { get; set; }
        public DateTime Datum { get; set; }
        public decimal Cena { get; set; } = 0;
        public int OsobaId { get; set; }
        public string Status { get; set; } = "Aktivan";
        public Racun() { }

        public Racun(int racunId, DateTime datum, decimal cena, int osobaId, Osoba osoba)
        {
            RacunId = racunId;
            Datum = datum;
            Cena = cena;
            OsobaId = osobaId;
        }

        public override string? ToString()
        {
            return "Racun: " + RacunId + "\nDatum: " + Datum.ToString("dd.MM.yyyy HH:mm") +
           "\nCena: " + Cena.ToString("F2");
        }
    }
}
