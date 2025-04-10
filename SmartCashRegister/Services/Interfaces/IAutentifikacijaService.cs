using SmartCashRegister.Models;

namespace SmartCashRegister.Services.Interfaces
{
    public interface IAutentifikacijaService
    {
        Osoba? UspesnaPrijava(string username, string password);
    }
}
