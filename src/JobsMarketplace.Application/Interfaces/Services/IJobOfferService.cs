using JobsMarketplace.Application.DTOs.Job;
using JobsMarketplace.Application.DTOs.JobOffer;
using JobsMarketplace.Domain.Entities;

namespace JobsMarketplace.Application.Interfaces.Services
{
    public interface IJobOfferService
    {
        Task<Guid> CreateAsync(CreateJobOfferRequest request);

        Task UpdateAsync(Guid id, UpdateJobOfferRequest request);

        Task DeleteAsync(Guid id);

        Task<JobOfferDetailsResponse?> GetJobOfferDetailsAsync(Guid id);

        Task AcceptJobOfferAsync(Guid offerId);


    }
}
