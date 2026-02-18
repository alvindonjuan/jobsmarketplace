using JobsMarketplace.Application.DTOs.Job;
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

namespace JobsMarketplace.Application.Tests.Jobs
{

        public class JobServiceTests
        {
            private readonly Mock<IJobRepository> _repositoryMock = new();
            private readonly Mock<IJobQuery> _queryMock = new();
            private readonly Mock<ICacheService> _cacheMock = new();

            private JobService CreateService()
                => new JobService(
                    _repositoryMock.Object,
                    _queryMock.Object,
                    _cacheMock.Object);

            [Fact]
            public async Task GetJobDetailsAsync_ShouldFetchFromDb_AndCache_WhenNotInCache()
            {
                // Arrange
                var id = Guid.NewGuid();
                var customerId = Guid.NewGuid();

            _cacheMock
                    .Setup(x => x.GetAsync<JobDetailsResponse>(It.IsAny<string>()))
                    .ReturnsAsync((JobDetailsResponse?)null);

                var dbJob = new JobDetailsResponse
                {
                    Id = id,
                    Title = "Senior Software Engineer",
                    Description = "Lorem ipsum dolor",
                    Budget = 50000,
                    Status = "Open",
                    CustomerId = customerId,
                    CustomerFirstName = "Alvin",
                    CustomerLastName = "Juan",
                    CreatedAt = DateTime.UtcNow
                };



            _queryMock
                    .Setup(x => x.GetJobDetailsAsync(id))
                    .ReturnsAsync(dbJob);

                var service = CreateService();

                // Act
                var result = await service.GetJobDetailsAsync(id);

                // Assert
                Assert.NotNull(result);
                Assert.Equal("Senior Software Engineer", result!.Title);
                Assert.Equal("Lorem ipsum dolor", result!.Description);

            _queryMock.Verify(
                    x => x.GetJobDetailsAsync(id),
                    Times.Once);

                _cacheMock.Verify(
                    x => x.SetAsync(
                        It.IsAny<string>(),
                        It.Is<JobDetailsResponse>(c => c.Id == id),
                        null),
                    Times.Once);
            }

            [Fact]
            public async Task CreateAsync_ShouldCreateJob_AndReturnId()
            {
                // Arrange

                var service = CreateService();
                var customerId = Guid.NewGuid();

                var request = new CreateJobRequest
                {
                    CustomerId = customerId,
                    Title = "Senior Software Engineer",
                    Description = "Lorem ipsum dolor",
                    Budget = 50000
                };

                Job? capturedJob = null;

                _repositoryMock
                    .Setup(x => x.CreateAsync(It.IsAny<Job>()))
                    .Callback<Job>(c => capturedJob = c)
                    .ReturnsAsync(Guid.NewGuid());

                // Act
                var result = await service.CreateAsync(request);

                // Assert
                Assert.NotEqual(Guid.Empty, result);

                _repositoryMock.Verify(
                    x => x.CreateAsync(It.IsAny<Job>()),
                    Times.Once);

                Assert.NotNull(capturedJob);
                Assert.Equal("Senior Software Engineer", capturedJob!.Title);
            }


            [Fact]
            public async Task UpdateAsync_ShouldUpdateJob_AndRemoveCache()
            {
                // Arrange
                var id = Guid.NewGuid();
                var customerId = Guid.NewGuid();
                var Job = new Job(customerId, "Senior Software Engineer", "Lorem ipsum dolor", 50000);

                _repositoryMock
                    .Setup(x => x.GetByIdAsync(id))
                    .ReturnsAsync(Job);

                var service = CreateService();

                string newTitle = "Junior Quality Assurance";
                string newDescription = "Lorem ipsum dolor";
                decimal newBudget = 20000;


                var request = new UpdateJobRequest
                {
                    Title = newTitle,
                    Description = newDescription,
                    Budget = newBudget
                };

                // Act
                await service.UpdateAsync(id, request);

                // Assert
                Assert.Equal(newTitle, Job.Title);
                Assert.Equal(newDescription, Job.Description);

                _repositoryMock.Verify(
                    x => x.UpdateAsync(Job),
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
