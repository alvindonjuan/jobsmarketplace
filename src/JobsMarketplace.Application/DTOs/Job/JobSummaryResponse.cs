namespace JobsMarketplace.Application.DTOs.Job
{
    public class JobSummaryResponse
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = default!;
        public decimal Budget { get; init; }
        public string Status { get; init; } = default!;
        public Guid CustomerId { get; init; }
        public string CustomerFirstName { get; init; } = default!;
        public string CustomerLastName { get; init; } = default!;
        public DateTimeOffset CreatedAt { get; init; }
    }


}
