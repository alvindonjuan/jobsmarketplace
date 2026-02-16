
namespace JobsMarketplace.Application.DTOs.Job
{
    public class UpdateJobRequest
    {
        public string Title { get; init; } = default!;
        public string Description { get; init; } = default!;
        public decimal Budget { get; init; }
    }

}
