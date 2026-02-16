using JobsMarketplace.Application.DTOs.Contractor;

namespace JobsMarketplace.Application.Interfaces.Services
{
    public interface IContractorService
    {
        Task<ContractorResponse?> GetByIdAsync(Guid id);

        Task<Guid> CreateAsync(CreateContractorRequest request);

        Task UpdateAsync(Guid id, UpdateContractorRequest request);

        Task DeleteAsync(Guid id);

        Task UpdateRatingAsync(Guid id, decimal newRating);

        Task<IEnumerable<ContractorResponse>> SearchContractorsAsync(SearchContractorsRequest request);
    }
}
