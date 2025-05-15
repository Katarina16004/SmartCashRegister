using SmartCashRegister.Models;

namespace SmartCashRegister.Services.Interfaces
{
    public interface IPodesavanjeProfilaService
    {
        public bool SacuvajPromeneAkoPostoje(Osoba osoba, string novoIme, string novoPrezime, string noviTelefon, string noviUsername, string novaLozinka);
    }
}
