namespace JobsMarketplace.Application.DTOs.Contractor
{
    public class SearchContractorsRequest
    {
        public string? Search { get; init; }
        public DateTimeOffset? LastCreatedAt { get; init; }
    }

}
