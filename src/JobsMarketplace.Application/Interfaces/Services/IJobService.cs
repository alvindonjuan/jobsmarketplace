using JobsMarketplace.Application.DTOs.Job;

namespace JobsMarketplace.Application.Interfaces.Services
{
    public interface IJobService
    {
        Task<Guid> CreateAsync(CreateJobRequest request);

        Task UpdateAsync(Guid id, UpdateJobRequest request);

        Task DeleteAsync(Guid id);

        Task<JobDetailsResponse?> GetJobDetailsAsync(Guid id);

        Task<IEnumerable<JobSummaryResponse>> SearchJobsAsync(SearchJobsRequest request);
    }
}
