using SmartCashRegister.Models;

namespace SmartCashRegister.Services.Interfaces
{
    public interface IUpravljanjeRacunomService
    {
        public bool IzveziUPDF(Racun racun);
        public bool StornirajRacun(Racun racun);
    }
}
