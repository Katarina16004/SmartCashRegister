using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCashRegister.Models
{
    public class Proizvod
    {
        public int ProizvodId { get; set; }
        public string? Naziv { get; set; } = string.Empty;
        public decimal Cena { get; set; } = 0;
        public int Kolicina { get; set; } = 0;
        public string? Barkod { get; set; } = string.Empty;
        public int KategorijaId { get; set; }
        public Kategorija? Kategorija { get; set; }

        public Proizvod()
        {
        }

        public Proizvod(int proizvodId, string naziv, decimal cena, int kolicina, string barkod, int kategorijaId)
        {
            ProizvodId = proizvodId;
            Naziv = naziv;
            Cena = cena;
            Kolicina = kolicina;
            Barkod = barkod;
            KategorijaId = kategorijaId;
        }

        public override string? ToString()
        {
            return "Proizvod: " + ProizvodId + "\tNaziv: " + Naziv + "\nCena: " + Cena + "\tKolicina: " + Kolicina + "\nBarKod: " + Barkod + "\nKategorijaID: " + KategorijaId;
        }
    }
}
