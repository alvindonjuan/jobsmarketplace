using JobsMarketplace.Application.Common.Caching;
using JobsMarketplace.Application.DTOs.Contractor;
using JobsMarketplace.Application.Interfaces.Queries;
using JobsMarketplace.Application.Interfaces.Repositories;
using JobsMarketplace.Application.Interfaces.Services;
using JobsMarketplace.Domain.Entities;

namespace JobsMarketplace.Application.Services
{
    public class ContractorService : IContractorService
    {
        private readonly IContractorRepository _repository;
        private readonly IContractorQuery _queries;
        private readonly ICacheService _cache;

        public ContractorService(IContractorRepository contractorRepository, IContractorQuery query, ICacheService cache)
        {
            _repository = contractorRepository;
            _queries = query;
            _cache = cache;

        }

        public async Task<ContractorResponse?> GetByIdAsync(Guid id)
        {
            var cacheKey = CacheKeys.Contractor(id);

            var cached = await _cache.GetAsync<ContractorResponse>(cacheKey);
            if (cached is not null)
                return cached;

            var contractor = await _repository.GetByIdAsync(id);

            if (contractor is null)
                return null;

            var response = new ContractorResponse
            { 
              Id =  contractor.Id,
              Name =  contractor.Name,
              Rating = contractor.Rating,
              CreatedAt =  contractor.CreatedAt
            };

            await _cache.SetAsync(cacheKey, response);

            return response;
        }

        public async Task<Guid> CreateAsync(CreateContractorRequest request)
        {
            var contractor = new Contractor(request.Name, 0); //Unrated
            return await _repository.CreateAsync(contractor);
        }

        public async Task UpdateAsync(Guid id, UpdateContractorRequest request)
        {

            var contractor = await _repository.GetByIdAsync(id);

            if (contractor is null)
                throw new Exception($"Contractor {id} not found.");

            contractor.UpdateName(request.Name);

            await _repository.UpdateAsync(contractor);

            var cacheKey = CacheKeys.Contractor(id);
            await _cache.RemoveAsync(cacheKey);

        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);

            var cacheKey = CacheKeys.Contractor(id);
            await _cache.RemoveAsync(cacheKey);
        }

        public async Task UpdateRatingAsync(Guid id, decimal newRating)
        {
            var contractor = await _repository.GetByIdAsync(id);

            if (contractor is null)
                throw new Exception("Contractor not found");

            contractor.UpdateRating(newRating);

            await _repository.UpdateAsync(contractor);

            var cacheKey = CacheKeys.Contractor(id);
            await _cache.RemoveAsync(cacheKey);
        }

        public async Task<IEnumerable<ContractorResponse>> SearchContractorsAsync(SearchContractorsRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.Search))
                return Enumerable.Empty<ContractorResponse>();

            return await _queries.SearchContractorsAsync(request.Search, request.LastCreatedAt);

        }



    }

}
