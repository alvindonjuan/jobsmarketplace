using JobsMarketplace.Domain.Entities;

namespace JobsMarketplace.Application.Interfaces.Repositories
{
    public interface IContractorRepository
    {
        Task<Contractor?> GetByIdAsync(Guid id);

        Task<Guid> CreateAsync(Contractor customer);

        Task UpdateAsync(Contractor contractor);

        Task DeleteAsync(Guid id);


    }
}
