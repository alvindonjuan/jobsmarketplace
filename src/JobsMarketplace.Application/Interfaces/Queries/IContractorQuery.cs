using JobsMarketplace.Application.DTOs.Contractor;
using JobsMarketplace.Application.DTOs.Customer;

namespace JobsMarketplace.Application.Interfaces.Queries
{
    public interface IContractorQuery
    {

        Task<ContractorResponse> GetByIdAsync(Guid id);
        Task<IEnumerable<ContractorResponse>> SearchContractorsAsync(string query, DateTimeOffset? lastCreatedAt);

    }

}
