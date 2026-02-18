using JobsMarketplace.Application.DTOs.Contractor;
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

namespace JobsMarketplace.Application.Tests.Contractors
{
    public class ContractorServiceTests
    {
        private readonly Mock<IContractorRepository> _repositoryMock = new();
        private readonly Mock<IContractorQuery> _queryMock = new();
        private readonly Mock<ICacheService> _cacheMock = new();

        private ContractorService CreateService()
            => new ContractorService(
                _repositoryMock.Object,
                _queryMock.Object,
                _cacheMock.Object);


        [Fact]
        public async Task GetByIdAsync_ShouldFetchFromDb_AndCache_WhenNotInCache()
        {
            // Arrange
            var id = Guid.NewGuid();

            _cacheMock
                .Setup(x => x.GetAsync<ContractorResponse>(It.IsAny<string>()))
                .ReturnsAsync((ContractorResponse?)null);

            var dbContractor = new ContractorResponse
            {
                Id = id,
                Name = "ABC Company",
                CreatedAt = DateTime.UtcNow
            };

            _queryMock
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(dbContractor);

            var service = CreateService();

            // Act
            var result = await service.GetByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("ABC Company", result!.Name);

            _queryMock.Verify(
                x => x.GetByIdAsync(id),
                Times.Once);

            _cacheMock.Verify(
                x => x.SetAsync(
                    It.IsAny<string>(),
                    It.Is<ContractorResponse>(c => c.Id == id),
                    null),
                Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateContractor_AndReturnId()
        {
            // Arrange

            var service = CreateService();

            var request = new CreateContractorRequest
            {
                Name = "ABC Company"
            };

            Contractor? capturedContractor = null;

            _repositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Contractor>()))
                .Callback<Contractor>(c => capturedContractor = c)
                .ReturnsAsync(Guid.NewGuid());

            // Act
            var result = await service.CreateAsync(request);

            // Assert
            Assert.NotEqual(Guid.Empty, result);

            _repositoryMock.Verify(
                x => x.CreateAsync(It.IsAny<Contractor>()),
                Times.Once);

            Assert.NotNull(capturedContractor);
            Assert.Equal("ABC Company", capturedContractor!.Name);
        }


        [Fact]
        public async Task UpdateAsync_ShouldUpdateContractor_AndRemoveCache()
        {
            // Arrange
            var id = Guid.NewGuid();

            var Contractor = new Contractor("ABC Company", 0);

            _repositoryMock
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(Contractor);

            var service = CreateService();

            string newName = "XYZ Company";

            var request = new UpdateContractorRequest
            {
                Name = newName
            };

            // Act
            await service.UpdateAsync(id, request);

            // Assert
            Assert.Equal(newName, Contractor.Name);

            _repositoryMock.Verify(
                x => x.UpdateAsync(Contractor),
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
