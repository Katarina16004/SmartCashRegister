using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCashRegister.Services
{
    public interface IAutentifikacijaService
    {
        bool UspesnaPrijava(string username, string password);
    }
}
