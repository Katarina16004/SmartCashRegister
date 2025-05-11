using SmartCashRegister.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCashRegister.Services.Interfaces
{
    public interface IUredjivanjeZaposlenihService
    {
        public IEnumerable<Osoba> PrikaziSveZaposlene();
        public IEnumerable<Osoba> PretraziZaposlenog(string ime = "", string prezime = "", string username = "");
        public bool DodajZaposlenog(Osoba o);
        public bool ObrisiZaposlenog(int osobaId);
    }
}
