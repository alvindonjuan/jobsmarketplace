
using JobsMarketplace.Domain.Entities;

namespace JobsMarketplace.Application.Interfaces.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByIdAsync(Guid id);

        Task<Guid> CreateAsync(Customer customer);

        Task UpdateAsync(Customer request);

        Task DeleteAsync(Guid id);

    }
}
