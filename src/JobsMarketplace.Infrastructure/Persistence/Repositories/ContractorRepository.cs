using Dapper;
using JobsMarketplace.Application.Interfaces.Repositories;
using JobsMarketplace.Domain.Entities;
using Npgsql;

namespace JobsMarketplace.Infrastructure.Persistence.Repositories
{
    public class ContractorRepository : IContractorRepository
    {
        private readonly IDbConnectionFactory _factory;

        private const int DefaultLimit = 50;
        public ContractorRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<Contractor?> GetByIdAsync(Guid id)
        {
            using var connection = _factory.CreateConnection();

            const string sql = """
            SELECT id,
                name AS Name,
                rating AS Rating,
                created_at AS CreatedAt,
                updated_at AS UpdatedAt
            FROM contractors
            WHERE id = @Id
            """;

            return await connection.QueryFirstOrDefaultAsync<Contractor>(
                sql,
                new { Id = id });
        }

       
        public async Task<Guid> CreateAsync(Contractor contractor)
        {
            using var connection = _factory.CreateConnection();

            const string sql = """
                INSERT INTO contractors
                (
                    id,
                    name,
                    rating,
                    created_at,
                    updated_at
                )
                VALUES
                (
                    @Id,
                    @Name,
                    @Rating,
                    @CreatedAt,
                    @UpdatedAt
                )
                """;

            await connection.ExecuteAsync(sql, new
            {
                contractor.Id,
                contractor.Name,
                contractor.Rating,
                contractor.CreatedAt,
                contractor.UpdatedAt
            });

            return contractor.Id;
        }

        public async Task UpdateAsync(Contractor contractor)
        {
            using var connection = _factory.CreateConnection();

            const string sql = """
                UPDATE contractors
                SET 
                    name = @Name,
                    rating = @Rating,
                    updated_at = @UpdatedAt
                WHERE id = @Id
            """;

            await connection.ExecuteAsync(sql, new
            {
                contractor.Id,
                contractor.Name,
                contractor.Rating,
                contractor.UpdatedAt
            });
        }




        public async Task DeleteAsync(Guid id)
        {
            using var connection = _factory.CreateConnection();

            const string sql = """
            DELETE FROM contractors
            WHERE id = @Id
            """;

            try
            {
                await connection.ExecuteAsync(sql, new { Id = id });

            }
            catch (PostgresException ex)
            {
                switch (ex.SqlState)
                {
                    //Handle only 1 for now
                    case PostgresErrorCodes.ForeignKeyViolation:
                        throw new InvalidOperationException(
                            "Cannot delete because related records exist.");

                    default:
                        throw;
                }
            }
        }


    }
}
