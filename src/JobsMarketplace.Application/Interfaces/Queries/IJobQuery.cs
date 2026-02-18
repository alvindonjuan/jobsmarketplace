using JobsMarketplace.Application.DTOs.Job;
using System.Threading.Tasks;

namespace JobsMarketplace.Application.Interfaces.Queries
{
    public interface IJobQuery
    {
        Task<JobDetailsResponse?> GetJobDetailsAsync(Guid id);

        Task<IEnumerable<JobSummaryResponse>> SearchJobsAsync(string query, DateTimeOffset? lastCreatedAt);

    }

}
 