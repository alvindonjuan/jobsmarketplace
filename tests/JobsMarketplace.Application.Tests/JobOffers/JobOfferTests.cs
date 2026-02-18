using JobsMarketplace.Application.DTOs.JobOffer;
using JobsMarketplace.Application.Interfaces.Queries;
using JobsMarketplace.Application.Interfaces.Repositories;
using JobsMarketplace.Application.Interfaces.Services;
using JobsMarketplace.Application.Services;
using JobsMarketplace.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsMarketplace.Application.Tests.JobOffers
{

    public class JobOfferTests
    {
        private readonly Mock<IJobOfferRepository> _repositoryMock = new();
        private readonly Mock<IJobOfferQuery> _queryMock = new();
        private readonly Mock<ICacheService> _cacheMock = new();

        private JobOfferService CreateService()
            => new JobOfferService(
                _repositoryMock.Object,
                _queryMock.Object,
                _cacheMock.Object);

        [Fact]
        public async Task GetJobOfferDetailsAsync_ShouldFetchFromDb_AndCache_WhenNotInCache()
        {
            // Arrange
            var id = Guid.NewGuid();
            var jobId = Guid.NewGuid();
            var contractorId = Guid.NewGuid();

            _cacheMock
                    .Setup(x => x.GetAsync<JobOfferDetailsResponse>(It.IsAny<string>()))
                    .ReturnsAsync((JobOfferDetailsResponse?)null);

            var dbJobOffer = new JobOfferDetailsResponse
            {
                Id = id,
                OfferedPrice = 50000,
                IsAccepted = false,
                CreatedAt = DateTime.UtcNow,
                ContractorId = contractorId,
                ContractorName = "ABC Company",
                JobId = jobId,
                JobTitle = "Senior Software Engineer",
                JobDescription = "Lorem ipsum dolor"
            };

        _queryMock
                    .Setup(x => x.GetJobOfferDetailsAsync(id))
                    .ReturnsAsync(dbJobOffer);

            var service = CreateService();

            // Act
            var result = await service.GetJobOfferDetailsAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("ABC Company", result!.ContractorName);
            Assert.Equal("Senior Software Engineer", result!.JobTitle);
            Assert.Equal("Lorem ipsum dolor", result!.JobDescription);

            _queryMock.Verify(
                    x => x.GetJobOfferDetailsAsync(id),
                    Times.Once);

            _cacheMock.Verify(
                x => x.SetAsync(
                    It.IsAny<string>(),
                    It.Is<JobOfferDetailsResponse>(c => c.Id == id),
                    null),
                Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateJobOffer_AndReturnId()
        {
            // Arrange

            var service = CreateService();
            var jobId = Guid.NewGuid();
            var contractorId = Guid.NewGuid();

            var request = new CreateJobOfferRequest
            {
                JobId = jobId,
                ContractorId = contractorId,
                OfferedPrice = 50000
            };

            JobOffer? capturedJobOffer = null;

            _repositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<JobOffer>()))
                .Callback<JobOffer>(c => capturedJobOffer = c)
                .ReturnsAsync(Guid.NewGuid());

            // Act
            var result = await service.CreateAsync(request);

            // Assert
            Assert.NotEqual(Guid.Empty, result);

            _repositoryMock.Verify(
                x => x.CreateAsync(It.IsAny<JobOffer>()),
                Times.Once);

            Assert.NotNull(capturedJobOffer);
            Assert.Equal(contractorId, capturedJobOffer!.ContractorId);
            Assert.Equal(jobId, capturedJobOffer!.JobId);
            Assert.Equal(50000, capturedJobOffer!.OfferedPrice);
        }


        [Fact]
        public async Task UpdateAsync_ShouldUpdateJobOffer_AndRemoveCache()
        {
            // Arrange
            var id = Guid.NewGuid();
            var jobId = Guid.NewGuid();
            var contractorId = Guid.NewGuid();
            var JobOffer = new JobOffer(jobId,contractorId, 50000);

            _repositoryMock
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(JobOffer);

            var service = CreateService();


            decimal newOfferedPrice = 20000;


            var request = new UpdateJobOfferRequest
            {
                OfferedPrice = newOfferedPrice
            };

            // Act
            await service.UpdateAsync(id, request);

            // Assert
            Assert.Equal(newOfferedPrice, JobOffer.OfferedPrice);

            _repositoryMock.Verify(
                x => x.UpdateAsync(JobOffer),
                Times.Once);

            _cacheMock.Verify(
                x => x.RemoveAsync(It.IsAny<string>()),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepository_AndRemoveCache()
        {
            // Arrange
            var id = Guid.NewGuid();

            var service = CreateService();

            _repositoryMock
                .Setup(x => x.DeleteAsync(id))
                .Returns(Task.CompletedTask);

            // Act
            await service.DeleteAsync(id);

            // Assert
            _repositoryMock.Verify(
                x => x.DeleteAsync(id),
                Times.Once);

            _cacheMock.Verify(
                x => x.RemoveAsync(It.IsAny<string>()),
                Times.Once);
        }


    }

}
