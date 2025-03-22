using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using System.IO;

namespace SmartCashRegister.Services
{
    public class PristupBaziService: IPristupBaziService
    {
        private readonly string? connectionString;

        public PristupBaziService()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) 
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            connectionString = config.GetConnectionString("SqlConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string is missing.");
            }
        }

        public DataTable ExecuteQuery(string query, SqlParameter[]? parameters = null)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        public int ExecuteNonQuery(string query, SqlParameter[]? parameters = null)
        {
            int rowsAffected = 0;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    con.Open();
                    rowsAffected = cmd.ExecuteNonQuery();
                }
            }
            return rowsAffected;
        }
    }
}
