
namespace JobsMarketplace.Application.DTOs.Customer
{
    public class SearchCustomersRequest
    {
        public string? Search { get; init; }
        public DateTimeOffset? LastCreatedAt { get; init; }
    }

}
