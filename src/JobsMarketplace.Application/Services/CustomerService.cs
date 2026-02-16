using JobsMarketplace.Application.Common.Caching;
using JobsMarketplace.Application.DTOs.Customer;
using JobsMarketplace.Application.Interfaces.Queries;
using JobsMarketplace.Application.Interfaces.Repositories;
using JobsMarketplace.Application.Interfaces.Services;
using JobsMarketplace.Domain.Entities;

namespace JobsMarketplace.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;

        private readonly ICustomerQuery _queries;

        private readonly ICacheService _cache;
        public CustomerService(ICustomerRepository repository, ICustomerQuery query, ICacheService cache)
        {
            _repository = repository;
            _queries = query;
            _cache = cache;
        }

        public async Task<CustomerResponse?> GetByIdAsync(Guid id)
        {
            var cacheKey = CacheKeys.Customer(id);

            var cached = await _cache.GetAsync<CustomerResponse>(cacheKey);
            if (cached is not null)
                return cached;

            var customer = await _repository.GetByIdAsync(id);

            if (customer is null)
                return null;

            var response = new CustomerResponse
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                CreatedAt = customer.CreatedAt
            };

            await _cache.SetAsync(cacheKey, response);

            return response;
        }

        public async Task<Guid> CreateAsync(CreateCustomerRequest request)
        {
            var customer = new Customer(request.FirstName, request.LastName);
            return await _repository.CreateAsync(customer);
        }

        public async Task UpdateAsync(Guid id, UpdateCustomerRequest request)
        {

            var customer = await _repository.GetByIdAsync(id);

            if (customer is null)
                throw new Exception($"Customer {id} not found.");

            customer.UpdateName(
                request.FirstName,
                request.LastName);

            await _repository.UpdateAsync(customer);

            var cacheKey = CacheKeys.Customer(id);
            await _cache.RemoveAsync(cacheKey);

        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
            var cacheKey = CacheKeys.Customer(id);
            await _cache.RemoveAsync(cacheKey);
        }


        public async Task<IEnumerable<CustomerResponse>> SearchCustomersAsync(SearchCustomersRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Search))
                return Enumerable.Empty<CustomerResponse>();

            return  await _queries.SearchCustomersAsync(request.Search, request.LastCreatedAt);

        }
    


    
    }

}
