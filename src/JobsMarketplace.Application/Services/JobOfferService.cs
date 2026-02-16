using JobsMarketplace.Application.DTOs.Job;
using JobsMarketplace.Application.DTOs.JobOffer;
using JobsMarketplace.Application.Interfaces.Queries;
using JobsMarketplace.Application.Interfaces.Repositories;
using JobsMarketplace.Application.Interfaces.Services;
using JobsMarketplace.Domain.Entities;

namespace JobsMarketplace.Application.Services
{
    public class JobOfferService : IJobOfferService
    {
        private readonly IJobOfferRepository _repository;

        private readonly IJobOfferQuery _query;

        public JobOfferService(IJobOfferRepository jobOfferRepository, IJobOfferQuery query)
        {
            _repository = jobOfferRepository;
            _query = query;
        }

        public async Task<Guid> CreateAsync(CreateJobOfferRequest request)
        {
            var jobOffer = new JobOffer(request.JobId, request.ContractorId, request.OfferedPrice);

            return await _repository.CreateAsync(jobOffer);

        }

        public async Task UpdateAsync(Guid id, UpdateJobOfferRequest request)
        {

            var jobOffer = await _repository.GetByIdAsync(id);

            if (jobOffer is null)
                throw new Exception($"Job Offer {id} not found.");

            jobOffer.UpdateOfferedPrice(request.OfferedPrice);

            await _repository.UpdateAsync(jobOffer);

        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }


        public async Task<JobOfferDetailsResponse?> GetJobOfferDetailsAsync(Guid id)
        {
            var jobOfferDetails = await _query.GetJobOfferDetailsAsync(id);

            if (jobOfferDetails is null)
                return null;

            return new JobOfferDetailsResponse
            {
                Id = jobOfferDetails.Id,
                OfferedPrice =jobOfferDetails.OfferedPrice,
                IsAccepted = jobOfferDetails.IsAccepted,
                CreatedAt = jobOfferDetails.CreatedAt,
                ContractorId = jobOfferDetails.ContractorId,
                ContractorName = jobOfferDetails.ContractorName,
                JobId = jobOfferDetails.JobId,
                JobTitle = jobOfferDetails.JobTitle,
                JobDescription = jobOfferDetails.JobDescription
            };

        }



        public async Task AcceptJobOfferAsync(Guid offerId)
        {
            var jobOffer = await _repository.GetByIdAsync(offerId);

            if (jobOffer is null)
                throw new Exception("Job offer not found");

            jobOffer.Accept();

            await _repository.UpdateAsync(jobOffer);
        }
    }
}
