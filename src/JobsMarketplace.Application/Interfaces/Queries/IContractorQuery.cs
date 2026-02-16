using JobsMarketplace.Application.DTOs.Contractor;

namespace JobsMarketplace.Application.Interfaces.Queries
{
    public interface IContractorQuery
    {
        Task<IEnumerable<ContractorResponse>> SearchContractorsAsync(string query, DateTimeOffset? lastCreatedAt);

    }

}
