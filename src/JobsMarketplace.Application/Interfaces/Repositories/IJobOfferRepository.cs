using JobsMarketplace.Domain.Entities;

namespace JobsMarketplace.Application.Interfaces.Repositories
{
    public interface IJobOfferRepository
    {
        Task<JobOffer?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(JobOffer id);
        Task UpdateAsync(JobOffer job);
        Task DeleteAsync(Guid id);
    }
}
