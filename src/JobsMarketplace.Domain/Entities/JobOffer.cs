using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsMarketplace.Domain.Entities
{
    public class JobOffer
    {
        public Guid Id { get; private set; }

        public Guid JobId { get; private set; }
        public Guid ContractorId { get; private set; }

        public decimal OfferedPrice { get; private set; }

        public bool IsAccepted { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset UpdatedAt { get; private set; }

        private JobOffer() { }

        public JobOffer(Guid jobId, Guid contractorId, decimal offeredPrice)
        {
            if (offeredPrice <= 0)
                throw new ArgumentException("Offered price must be greater than zero");

            Id = Guid.NewGuid();
            JobId = jobId;
            ContractorId = contractorId;
            OfferedPrice = offeredPrice;
            IsAccepted = false;
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public void UpdateOfferedPrice(decimal offeredPrice)
        {
            if (offeredPrice <= 0)
                throw new ArgumentException("Offered price must be greater than zero");

            OfferedPrice = offeredPrice;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public void Accept()
        {
            IsAccepted = true;
            UpdatedAt = DateTimeOffset.UtcNow;
        }
    }

}
