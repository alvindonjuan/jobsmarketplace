using JobsMarketplace.Application.Common.Caching;
using JobsMarketplace.Application.DTOs.Customer;
using JobsMarketplace.Application.DTOs.Job;
using JobsMarketplace.Application.Interfaces.Queries;
using JobsMarketplace.Application.Interfaces.Repositories;
using JobsMarketplace.Application.Interfaces.Services;
using JobsMarketplace.Domain.Entities;

namespace JobsMarketplace.Application.Services
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _repository;
        private readonly IJobQuery _query;
        private readonly ICacheService _cache;

        public JobService(IJobRepository jobRepository, IJobQuery query, ICacheService cache)
        {
            _repository = jobRepository;
            _query = query;
            _cache = cache;
        }

        public async Task<Guid> CreateAsync(CreateJobRequest request)
        {
            var job = new Job(request.CustomerId, request.Title, request.Description, request.Budget);

            return await _repository.CreateAsync(job);

        }

        public async Task UpdateAsync(Guid id, UpdateJobRequest request)
        {
            var job = await _repository.GetByIdAsync(id);

            if (job is null)
                throw new Exception($"Job {id} not found.");

            job.UpdateJob(request.Title,request.Description, request.Budget);

            await _repository.UpdateAsync(job);

            var cacheKey = CacheKeys.JobDetails(id);
            await _cache.RemoveAsync(cacheKey);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
            var cacheKey = CacheKeys.JobDetails(id);
            await _cache.RemoveAsync(cacheKey);
        }


        public async Task<JobDetailsResponse?> GetJobDetailsAsync(Guid id)
        {
            var cacheKey = CacheKeys.JobDetails(id);

            var cached = await _cache.GetAsync<JobDetailsResponse>(cacheKey);
            if (cached is not null)
                return cached;

            var jobDetails = await _query.GetJobDetailsAsync(id);

            if (jobDetails is null)
                return null;

            var response = new JobDetailsResponse
            {
                Id = jobDetails.Id,
                Title = jobDetails.Title,
                Description = jobDetails.Description,
                Budget = jobDetails.Budget,
                Status = jobDetails.Status,
                CustomerId = jobDetails.CustomerId,
                CustomerFirstName = jobDetails.CustomerFirstName,
                CustomerLastName = jobDetails.CustomerLastName,
                CreatedAt = jobDetails.CreatedAt
            };

            await _cache.SetAsync(cacheKey, response);

            return response;

        }


        public async Task<IEnumerable<JobSummaryResponse>> SearchJobsAsync(SearchJobsRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Search))
                return Enumerable.Empty<JobSummaryResponse>();

            return await _query.SearchJobsAsync(request.Search, request.LastCreatedAt);

        }







    }


}
