using SmartCashRegister.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCashRegister.Services.Interfaces
{
    public interface IUredjivanjeProizvodaService
    {
        public bool DodajProizvod(Proizvod p);
        public bool ObrisiProizvod(int proizvodId);
        //public bool IzmeniProizvod(Proizvod p);
    }
}
