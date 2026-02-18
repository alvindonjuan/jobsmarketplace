using JobsMarketplace.Application.Common.Caching;
using JobsMarketplace.Application.DTOs.Job;
using JobsMarketplace.Application.DTOs.JobOffer;
using JobsMarketplace.Application.Interfaces.Queries;
using JobsMarketplace.Application.Interfaces.Repositories;
using JobsMarketplace.Application.Interfaces.Services;
using JobsMarketplace.Domain.Entities;

namespace JobsMarketplace.Application.Services
{
    public class JobOfferService : IJobOfferService
    {
        private readonly IJobOfferRepository _repository;

        private readonly IJobOfferQuery _query;
        private readonly ICacheService _cache;

        public JobOfferService(IJobOfferRepository jobOfferRepository, IJobOfferQuery query, ICacheService cache)
        {
            _repository = jobOfferRepository;
            _query = query;
            _cache = cache;
        }

        public async Task<Guid> CreateAsync(CreateJobOfferRequest request)
        {
            var jobOffer = new JobOffer(request.JobId, request.ContractorId, request.OfferedPrice);

            return await _repository.CreateAsync(jobOffer);

        }

        public async Task UpdateAsync(Guid id, UpdateJobOfferRequest request)
        {

            var jobOffer = await _repository.GetByIdAsync(id);

            if (jobOffer is null)
                throw new Exception($"Job Offer {id} not found.");

            jobOffer.UpdateOfferedPrice(request.OfferedPrice);

            await _repository.UpdateAsync(jobOffer);

            var cacheKey = CacheKeys.JobOfferDetails(id);
            await _cache.RemoveAsync(cacheKey);


        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
            var cacheKey = CacheKeys.JobOfferDetails(id);
            await _cache.RemoveAsync(cacheKey);
        }


        public async Task<JobOfferDetailsResponse?> GetJobOfferDetailsAsync(Guid id)
        {
            var cacheKey = CacheKeys.JobOfferDetails(id);

            var cached = await _cache.GetAsync<JobOfferDetailsResponse>(cacheKey);
            if (cached is not null)
                return cached;


            var jobOfferDetails = await _query.GetJobOfferDetailsAsync(id);

            if (jobOfferDetails is null)
                return null;

            var response = new JobOfferDetailsResponse
            {
                Id = jobOfferDetails.Id,
                OfferedPrice = jobOfferDetails.OfferedPrice,
                IsAccepted = jobOfferDetails.IsAccepted,
                CreatedAt = jobOfferDetails.CreatedAt,
                ContractorId = jobOfferDetails.ContractorId,
                ContractorName = jobOfferDetails.ContractorName,
                JobId = jobOfferDetails.JobId,
                JobTitle = jobOfferDetails.JobTitle,
                JobDescription = jobOfferDetails.JobDescription
            };

            await _cache.SetAsync(cacheKey, response);

            return response;


        }



        public async Task AcceptJobOfferAsync(Guid id)
        {
            var jobOffer = await _repository.GetByIdAsync(id);

            if (jobOffer is null)
                throw new Exception("Job offer not found");

            jobOffer.Accept();

            await _repository.UpdateAsync(jobOffer);

            var cacheKey = CacheKeys.JobOfferDetails(id);
            await _cache.RemoveAsync(cacheKey);
        }
    }
}
