using Dapper;
using JobsMarketplace.Application.Interfaces.Repositories;
using JobsMarketplace.Domain.Entities;
using Npgsql;
using System.Data;

namespace JobsMarketplace.Infrastructure.Persistence.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IDbConnectionFactory _factory;

        public CustomerRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<Customer?> GetByIdAsync(Guid id)
        {
            using var connection = _factory.CreateConnection();

            const string sql = """
            SELECT id,
                first_name AS FirstName,
                last_name AS LastName,
                created_at AS CreatedAt,
                updated_at AS UpdatedAt
            FROM customers
            WHERE id = @Id
            """;

            return await connection.QueryFirstOrDefaultAsync<Customer>(
                sql,
                new { Id = id });
        }


        public async Task<Guid> CreateAsync(Customer customer)
        {
            using var connection = _factory.CreateConnection();

            const string sql = """
                INSERT INTO customers
                (
                    id,
                    first_name,
                    last_name,
                    created_at,
                    updated_at
                )
                VALUES
                (
                    @Id,
                    @FirstName,
                    @LastName,
                    @CreatedAt,
                    @UpdatedAt
                )
                """;

            await connection.ExecuteAsync(sql, new
            {
                customer.Id,
                customer.FirstName,
                customer.LastName,
                customer.CreatedAt,
                customer.UpdatedAt
            });

            return customer.Id;
        }

        public async Task UpdateAsync(Customer customer)
        {
            using var connection = _factory.CreateConnection();

            const string sql = """
                UPDATE customers
                SET
                    first_name = @FirstName,
                    last_name = @LastName,
                    updated_at = @UpdatedAt
                WHERE id = @Id
                """;

            await connection.ExecuteAsync(sql, new
            {
                customer.Id,
                customer.FirstName,
                customer.LastName,
                customer.UpdatedAt
            });
        }

        public async Task DeleteAsync(Guid id)
        {
            using var connection = _factory.CreateConnection();

            //NOTE: Ideally this should be soft delete, but this is ok for now :)

            const string sql = """
                DELETE FROM customers
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
