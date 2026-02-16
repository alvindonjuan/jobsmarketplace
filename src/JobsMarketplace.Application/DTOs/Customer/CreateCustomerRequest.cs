using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsMarketplace.Application.DTOs.Customer
{
    public class CreateCustomerRequest
    {
        public string FirstName { get; init; } = default!;
        public string LastName { get; init; } = default!;
    }

}
