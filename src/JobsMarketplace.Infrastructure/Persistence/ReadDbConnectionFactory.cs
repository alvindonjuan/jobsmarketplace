using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace JobsMarketplace.Infrastructure.Persistence
{


    public interface IReadDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }

    public class ReadDbConnectionFactory : IReadDbConnectionFactory
    {
        private readonly string _connectionString;

        public ReadDbConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }

}
