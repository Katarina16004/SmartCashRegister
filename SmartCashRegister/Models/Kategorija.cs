using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCashRegister.Models
{
    public class Kategorija
    {
        public int KategorijaId { get; set; }
        public string? Naziv { get; set; } = string.Empty;

        public Kategorija() { }

        public Kategorija(int id, string naziv)
        {
            KategorijaId = id;
            Naziv = naziv;
        }

        public override string? ToString()
        {
            return "Kategorija: " + KategorijaId + " Naziv: " + Naziv;
        }
    }
}
