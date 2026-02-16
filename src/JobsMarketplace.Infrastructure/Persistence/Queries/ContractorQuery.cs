using Dapper;
using JobsMarketplace.Application.DTOs.Contractor;
using JobsMarketplace.Application.Interfaces.Queries;
using JobsMarketplace.Domain.Entities;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsMarketplace.Infrastructure.Persistence.Queries
{
    public class ContractorQuery : IContractorQuery
    {
        private readonly IReadDbConnectionFactory _factory;

        private const int DefaultLimit = 50;

        public ContractorQuery(IReadDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<ContractorResponse>> SearchContractorsAsync(string query, DateTimeOffset? lastCreatedAt)
        {
            using var connection = _factory.CreateConnection();

            const string searchSql = """
            SELECT id,
                name AS Name,
                rating AS Rating,
                created_at AS CreatedAt,
                updated_at AS UpdatedAt
                FROM contractors
            WHERE name ILIKE @Query
            AND(@LastCreatedAt IS NULL
            OR created_at < @LastCreatedAt)
            ORDER BY created_at DESC
            LIMIT @Limit
        """;

            return await connection.QueryAsync<ContractorResponse>(searchSql
                , new { 
                    Query = $"%{query}%"
                    , LastCreatedAt = lastCreatedAt
                    , Limit = DefaultLimit });
        }

    }


    
      

    }
