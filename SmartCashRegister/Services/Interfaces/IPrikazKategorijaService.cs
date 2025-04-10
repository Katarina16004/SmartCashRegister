using SmartCashRegister.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCashRegister.Services.Interfaces
{
    public interface IPrikazKategorijaService
    {
        public IEnumerable<Kategorija> PrikaziSve();
    }
}
