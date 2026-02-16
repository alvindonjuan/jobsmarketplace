using JobsMarketplace.Domain.Entities;

namespace JobsMarketplace.Application.Interfaces.Repositories
{
    public interface IJobRepository
    {
        Task<Job?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(Job id);
        Task UpdateAsync(Job job);
        Task DeleteAsync(Guid id);
    }
}
