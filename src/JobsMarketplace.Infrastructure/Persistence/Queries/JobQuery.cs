using Dapper;
using JobsMarketplace.Application.DTOs.Job;
using JobsMarketplace.Application.Interfaces.Queries;
using JobsMarketplace.Domain.Entities;

namespace JobsMarketplace.Infrastructure.Persistence.Queries
{
    public class JobQuery : IJobQuery
    {
        private readonly IReadDbConnectionFactory _factory;

        private const int DefaultLimit = 50;
        public JobQuery(IReadDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<JobDetailsResponse?> GetJobDetailsAsync(Guid id)
        {
            using var connection = _factory.CreateConnection();

            const string sql = """
             SELECT
                 j.id,
                 j.title           AS Title,
                 j.description     AS Description,
                 j.budget          AS Budget,
                 j.status          AS Status,
                 j.customer_id     AS CustomerId,
                 j.created_at      AS CreatedAt,
                 c.first_name      AS CustomerFirstName,
                 c.last_name       AS CustomerLastName
             FROM jobs j
             INNER JOIN customers c
                 ON j.customer_id = c.id
             WHERE j.id = @Id
         """;

            return await connection.QueryFirstOrDefaultAsync<JobDetailsResponse>(
                sql,
                new { Id = id });
        }



        public async Task<IEnumerable<JobSummaryResponse>> SearchJobsAsync(string query, DateTimeOffset? lastCreatedAt)
        {
            using var connection = _factory.CreateConnection();


        const string sql = """
              SELECT
                 j.id,
                 j.title           AS Title,
                 j.budget          AS Budget,
                 j.status          AS Status,
                 j.customer_id     AS CustomerId,
                 c.first_name      AS CustomerFirstName,
                 c.last_name       AS CustomerLastName,
                 j.created_at      AS CreatedAt
                FROM jobs j
                INNER JOIN customers c
                 ON j.customer_id = c.id
                WHERE j.search_vector @@ plainto_tsquery('english', @Query)
                AND (@LastCreatedAt IS NULL 
                OR j.created_at < @LastCreatedAt)
                ORDER BY j.created_at DESC
                LIMIT @PageSize

         """;

            return await connection.QueryAsync<JobSummaryResponse>(sql
                , new {
                    Query = query
                    ,LastCreatedAt = lastCreatedAt, 
                    PageSize = DefaultLimit
            });
        }



        



    }
}
