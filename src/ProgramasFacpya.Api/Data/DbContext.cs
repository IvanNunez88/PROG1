using Microsoft.Data.SqlClient;
using System.Data;

namespace ProgramasFacpya.Api.Data;

public class DbContext(IConfiguration configuration)
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")!;

    public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
}
