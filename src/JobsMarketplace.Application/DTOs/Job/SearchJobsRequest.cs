
namespace JobsMarketplace.Application.DTOs.Job
{
    public class SearchJobsRequest
    {
        public string? Search { get; init; }
        public DateTimeOffset? LastCreatedAt { get; init; }
    }
}
