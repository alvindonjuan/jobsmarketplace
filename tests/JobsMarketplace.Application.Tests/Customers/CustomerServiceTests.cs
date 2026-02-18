using JobsMarketplace.Application.Common.Caching;
using JobsMarketplace.Application.DTOs.Customer;
using JobsMarketplace.Application.Interfaces.Queries;
using JobsMarketplace.Application.Interfaces.Repositories;
using JobsMarketplace.Application.Interfaces.Services;
using JobsMarketplace.Application.Services;
using JobsMarketplace.Domain.Entities;
using Moq;
using Xunit;

namespace JobsMarketplace.Application.Tests.Customers
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerRepository> _repositoryMock = new();
        private readonly Mock<ICustomerQuery> _queryMock = new();
        private readonly Mock<ICacheService> _cacheMock = new();

        private CustomerService CreateService()
            => new CustomerService(
                _repositoryMock.Object,
                _queryMock.Object,
                _cacheMock.Object);


        [Fact]
        public async Task GetByIdAsync_ShouldFetchFromDb_AndCache_WhenNotInCache()
        {
            // Arrange
            var id = Guid.NewGuid();

            _cacheMock
                .Setup(x => x.GetAsync<CustomerResponse>(It.IsAny<string>()))
                .ReturnsAsync((CustomerResponse?)null);

            var dbCustomer = new CustomerResponse
            {
                Id = id,
                FirstName = "Alvin",
                LastName = "Juan",
                CreatedAt = DateTime.UtcNow
            };

            _queryMock
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(dbCustomer);

            var service = CreateService();

            // Act
            var result = await service.GetByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Alvin", result!.FirstName);

            _queryMock.Verify(
                x => x.GetByIdAsync(id),
                Times.Once);

            _cacheMock.Verify(
                x => x.SetAsync(
                    It.IsAny<string>(),
                    It.Is<CustomerResponse>(c => c.Id == id),
                    null),
                Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateCustomer_AndReturnId()
        {
            // Arrange

            var service = CreateService();

            var request = new CreateCustomerRequest
            {
                FirstName = "Alvin",
                LastName = "Juan"
            };

            Customer? capturedCustomer = null;



            _repositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Customer>()))
                .Callback<Customer>(c => capturedCustomer = c)
                .ReturnsAsync(Guid.NewGuid());

            // Act
            var result = await service.CreateAsync(request);

            // Assert
            Assert.NotEqual(Guid.Empty, result);

            _repositoryMock.Verify(
                x => x.CreateAsync(It.IsAny<Customer>()),
                Times.Once);

            Assert.NotNull(capturedCustomer);
            Assert.Equal("Alvin", capturedCustomer!.FirstName);
            Assert.Equal("Juan", capturedCustomer!.LastName);
        }


        [Fact]
        public async Task UpdateAsync_ShouldUpdateCustomer_AndRemoveCache()
        {
            // Arrange
            var id = Guid.NewGuid();

            var customer = new Customer("Alvin", "Juan");

            _repositoryMock
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(customer);

            var service = CreateService();

            string newFirstName = "Pedro";
            string newLastName = "Dela Cruz";

            var request = new UpdateCustomerRequest
            {
                FirstName = newFirstName,
                LastName = newLastName
            };

            // Act
            await service.UpdateAsync(id, request);

            // Assert
            Assert.Equal(newFirstName, customer.FirstName);
            Assert.Equal(newLastName, customer.LastName);

            _repositoryMock.Verify(
                x => x.UpdateAsync(customer),
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