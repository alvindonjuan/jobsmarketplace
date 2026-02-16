using Dapper;
using JobsMarketplace.Application.DTOs.Customer;
using JobsMarketplace.Application.Interfaces.Queries;
using JobsMarketplace.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsMarketplace.Infrastructure.Persistence.Queries
{
    public class CustomerQuery : ICustomerQuery
    {
        private readonly IReadDbConnectionFactory _factory;

        private const int DefaultLimit = 50;

        public CustomerQuery(IReadDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<CustomerResponse>> SearchCustomersAsync(string query, DateTimeOffset? lastCreatedAt)
        {
            using var connection = _factory.CreateConnection();

            const string searchSql = """
            SELECT id,
                   first_name AS FirstName,
                   last_name AS LastName,
                   created_at AS CreatedAt
            FROM customers
            WHERE full_name ILIKE @Query
            AND (@LastCreatedAt IS NULL 
            OR created_at < @LastCreatedAt)
            ORDER BY created_at DESC
            LIMIT @Limit
        """;

            return await connection.QueryAsync<CustomerResponse>(searchSql
                    , new { 
                        Query = $"%{query}%"
                        , LastCreatedAt = lastCreatedAt
                        , Limit = DefaultLimit });
        }

    }
}
