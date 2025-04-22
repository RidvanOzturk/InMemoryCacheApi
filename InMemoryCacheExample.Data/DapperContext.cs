using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DapperCacheExample.Data;

public class DapperContext(IConfiguration configuration)
{
    private readonly string connectionString = configuration.GetConnectionString("DefaultConnection")!;
    public IDbConnection CreateConnection()
        => new SqlConnection(connectionString);
}
