using Dapper;
using JobsMarketplace.Application.DTOs.JobOffer;
using JobsMarketplace.Application.Interfaces.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsMarketplace.Infrastructure.Persistence.Queries
{


    public class JobOfferQuery : IJobOfferQuery
    {
        private readonly IReadDbConnectionFactory _factory;

        public JobOfferQuery(IReadDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<JobOfferDetailsResponse?> GetJobOfferDetailsAsync(Guid id)
        {
            using var connection = _factory.CreateConnection();

            const string sql = """
             SELECT
                 jo.id,
                 jo.job_id        AS JobId,
                 jo.contractor_id AS ContractorId,
                 jo.offered_price AS OfferedPrice,
                 jo.is_accepted   AS IsAccepted,
                 jo.created_at    AS CreatedAt,
                 con.name         AS ContractorName,
                 j.title          AS JobTitle,
                 j.description    AS JobDescription
             FROM job_offers jo
             INNER JOIN jobs j
                ON jo.job_id = j.id 
             INNER JOIN contractors con
                 ON jo.contractor_id = con.id
             WHERE jo.id = @Id
         """;

            return await connection.QueryFirstOrDefaultAsync<JobOfferDetailsResponse>(
                sql,
                new { Id = id });
        }

    }

}
