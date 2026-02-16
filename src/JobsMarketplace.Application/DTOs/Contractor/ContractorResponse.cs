using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsMarketplace.Application.DTOs.Contractor
{
    public class ContractorResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = default!;
        public decimal Rating { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
    }
}
