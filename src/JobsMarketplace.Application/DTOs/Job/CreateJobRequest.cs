
namespace JobsMarketplace.Application.DTOs.Job
{
    public class CreateJobRequest
    {
        public Guid CustomerId { get; init; }
        public string Title { get; init; } = default!;
        public string Description { get; init; } = default!;
        public decimal Budget { get; init; }
    }

}
