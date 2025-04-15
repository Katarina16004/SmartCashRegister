using SmartCashRegister.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SmartCashRegister.Services.Interfaces
{
    public interface IPretragaRacunaService
    {
        public IEnumerable<Racun> PrikaziSveRacune();
        public IEnumerable<Racun> PretraziRacun(string idRacuna = "", string username = "", DateTime? datum = null);
        public IEnumerable<StavkaRacuna> PrikaziStavkeRacuna(int racunId);
    }
}
