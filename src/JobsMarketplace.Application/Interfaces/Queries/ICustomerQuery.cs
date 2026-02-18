using JobsMarketplace.Application.DTOs.Customer;
using JobsMarketplace.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsMarketplace.Application.Interfaces.Queries
{
    public interface ICustomerQuery
    {
        Task<CustomerResponse> GetByIdAsync(Guid id);

        Task<IEnumerable<CustomerResponse>> SearchCustomersAsync(string query, DateTimeOffset? lastCreatedAt);
    }
}
