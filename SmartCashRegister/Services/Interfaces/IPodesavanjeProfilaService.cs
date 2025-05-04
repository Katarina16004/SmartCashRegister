using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCashRegister.Services.Interfaces
{
    public interface IPodesavanjeProfilaService
    {
        public bool PromeniIme(int prijavljeniId, string novoIme);
        public bool PromeniPrezime(int prijavljeniId, string novoPrezime);
        public bool PromeniTelefon(int prijavljeniId, string noviTelefon);
        public bool PromeniUsername(int prijavljeniId, string noviUsername);
        public bool PromeniLozinku(int prijavljeniId, string novaLozinka);
    }
}
