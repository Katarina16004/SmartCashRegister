using SmartCashRegister.Models;

namespace SmartCashRegister.Services
{
    public interface IAutentifikacijaService
    {
        Osoba? UspesnaPrijava(string username, string password);
    }
}
