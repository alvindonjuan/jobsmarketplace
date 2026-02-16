
namespace JobsMarketplace.Application.DTOs.JobOffer
{
    public class JobOfferDetailsResponse
    {
        public Guid Id { get; init; }
        public decimal OfferedPrice { get; init; }
        public bool IsAccepted { get; init; }
        public DateTimeOffset CreatedAt { get; init; }

        public Guid ContractorId { get; init; }
        public string ContractorName { get; init; } = default!;
        public Guid JobId { get; init; }
        public string JobTitle { get; init; } = default!;
        public string JobDescription { get; init; } = default!;
    }
}
