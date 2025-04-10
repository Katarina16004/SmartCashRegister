using SmartCashRegister.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCashRegister.Services.Interfaces
{
    public interface IPretragaProizvodaService
    {
        public IEnumerable<Proizvod> PrikaziSveProizvode();
        public IEnumerable<Proizvod> PretraziProizvod(string barKod = "", string naziv = "", string kategorija = "");
    }
}
