using SmartCashRegister.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCashRegister.Services.Interfaces
{
    public interface IKreiranjeRacunaService
    {
        public bool DodajProizvod(string barkod,string kol);
        public List<StavkaRacuna> GetStavkeRacuna();
    }
}
