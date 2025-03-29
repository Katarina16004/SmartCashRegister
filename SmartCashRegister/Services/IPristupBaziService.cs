using System.Data;
using Microsoft.Data.SqlClient;

namespace SmartCashRegister.Services
{
    public interface IPristupBaziService
    {
        DataTable ExecuteQuery(string query, SqlParameter[]? parameters = null);
        int ExecuteNonQuery(string query, SqlParameter[]? parameters = null);
    }
}
