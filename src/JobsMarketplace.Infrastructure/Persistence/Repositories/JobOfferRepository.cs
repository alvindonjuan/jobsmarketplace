using Dapper;
using JobsMarketplace.Application.Interfaces.Repositories;
using JobsMarketplace.Domain.Entities;
using Npgsql;


namespace JobsMarketplace.Infrastructure.Persistence.Repositories
{
    public class JobOfferRepository : IJobOfferRepository
    {
        private readonly IDbConnectionFactory _factory;

        public JobOfferRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<JobOffer?> GetByIdAsync(Guid id)
        {
            using var connection = _factory.CreateConnection();

            const string sql = """
            SELECT
                id,
                job_id        AS JobId,
                contractor_id AS ContractorId,
                offered_price AS OfferedPrice,
                is_accepted   AS IsAccepted,
                created_at    AS CreatedAt,
                updated_at    AS UpdatedAt
            FROM job_offers
            WHERE id = @Id
        """;

            return await connection.QueryFirstOrDefaultAsync<JobOffer>(
                sql,
                new { Id = id });
        }

        public async Task<Guid> CreateAsync(JobOffer offer)
        {
            using var connection = _factory.CreateConnection();

            const string sql = """
            INSERT INTO job_offers
            (
                id,
                job_id,
                contractor_id,
                offered_price,
                is_accepted,
                created_at,
                updated_at
            )
            VALUES
            (
                @Id,
                @JobId,
                @ContractorId,
                @OfferedPrice,
                @IsAccepted,
                @CreatedAt,
                @UpdatedAt
            )
        """;

            await connection.ExecuteAsync(sql, new
            {
                offer.Id,
                offer.JobId,
                offer.ContractorId,
                offer.OfferedPrice,
                offer.IsAccepted,
                offer.CreatedAt,
                offer.UpdatedAt
            });

            return offer.Id;
        }

        public async Task UpdateAsync(JobOffer offer)
        {
            using var connection = _factory.CreateConnection();

            const string sql = """
            UPDATE job_offers
            SET
                job_id = @JobId,
                contractor_id = @ContractorId,
                offered_price = @OfferedPrice,
                is_accepted = @IsAccepted,
                updated_at = @UpdatedAt
            WHERE id = @Id
        """;

            await connection.ExecuteAsync(sql, new
            {
                offer.Id,
                offer.JobId,
                offer.ContractorId,
                offer.OfferedPrice,
                offer.IsAccepted,
                offer.UpdatedAt
            });
        }

        public async Task DeleteAsync(Guid id)
        {
            using var connection = _factory.CreateConnection();

        //Note: This should be soft delete, but this is ok for now

            const string sql = """
            DELETE FROM job_offers
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
