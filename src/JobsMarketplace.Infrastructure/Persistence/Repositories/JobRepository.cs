using Dapper;
using JobsMarketplace.Application.Interfaces.Repositories;
using JobsMarketplace.Domain.Entities;
using Npgsql;

namespace JobsMarketplace.Infrastructure.Persistence.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly IDbConnectionFactory _factory;

        public JobRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<Job?> GetByIdAsync(Guid id)
        {
            using var connection = _factory.CreateConnection();

            const string sql = """
            SELECT
                id,
                customer_id      AS CustomerId,
                title            AS Title,
                description      AS Description,
                budget           AS Budget,
                status           AS Status,
                created_at       AS CreatedAt,
                updated_at       AS UpdatedAt,
                start_date       AS StartDate,
                due_date         AS DueDate
            FROM jobs
            WHERE id = @Id
            """;

            return await connection.QueryFirstOrDefaultAsync<Job>(
                sql,
                new { Id = id });
        }

        public async Task<Guid> CreateAsync(Job job)
        {
            using var connection = _factory.CreateConnection();

            const string sql = """
            INSERT INTO jobs
            (
                id,
                customer_id,
                title,
                description,
                budget,
                status,
                created_at,
                updated_at,
                start_date,
                due_date
            )
            VALUES
            (
                @Id,
                @CustomerId,
                @Title,
                @Description,
                @Budget,
                @Status,
                @CreatedAt,
                @UpdatedAt,
                @StartDate,
                @DueDate
            )
        """;

            await connection.ExecuteAsync(sql, new
            {
                job.Id,
                job.CustomerId,
                job.Title,
                job.Description,
                job.Budget,
                Status = (short)job.Status,
                job.CreatedAt,
                job.UpdatedAt,
                job.StartDate,
                job.DueDate
            });

            return job.Id;
        }

        public async Task UpdateAsync(Job job)
        {
            using var connection = _factory.CreateConnection();

            const string sql = """
            UPDATE jobs
            SET
                title = @Title,
                description = @Description,
                budget = @Budget,
                status = @Status,
                updated_at = @UpdatedAt,
                start_date = @StartDate,
                due_date = @DueDate
            WHERE id = @Id
        """;

            await connection.ExecuteAsync(sql, new
            {
                job.Id,
                job.Title,
                job.Description,
                job.Budget,
                Status = (short)job.Status,
                job.UpdatedAt,
                job.StartDate,
                job.DueDate
            });
        }

        public async Task DeleteAsync(Guid id)
        {
            using var connection = _factory.CreateConnection();

            //NOTE: Ideally this should be soft delete, but this is ok for now :)

            const string sql = """
            DELETE FROM jobs
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
