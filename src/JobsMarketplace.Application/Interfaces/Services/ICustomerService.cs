using JobsMarketplace.Application.DTOs.Customer;

namespace JobsMarketplace.Application.Interfaces.Services
{
    public interface ICustomerService
    {
        Task<CustomerResponse?> GetByIdAsync(Guid id);

        Task<Guid> CreateAsync(CreateCustomerRequest request);

        Task UpdateAsync(Guid id, UpdateCustomerRequest request);

        Task DeleteAsync(Guid id);

        Task<IEnumerable<CustomerResponse>> SearchCustomersAsync(SearchCustomersRequest request);

    } 

}
