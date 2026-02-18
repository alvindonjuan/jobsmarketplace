
using JobsMarketplace.Application.DTOs.Contractor;
using JobsMarketplace.Application.DTOs.JobOffer;

namespace JobsMarketplace.Application.Interfaces.Queries
{
    public interface IJobOfferQuery
    {
        Task<JobOfferDetailsResponse?> GetJobOfferDetailsAsync(Guid id);
    }

}
