
namespace JobsMarketplace.Application.DTOs.JobOffer
{
    public class CreateJobOfferRequest
    {
        public Guid JobId { get; init; }
        public Guid ContractorId { get; init; }
        public decimal OfferedPrice { get; init; }
    }

}
