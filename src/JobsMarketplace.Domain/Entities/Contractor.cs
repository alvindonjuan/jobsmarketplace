using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsMarketplace.Domain.Entities
{
    public class Contractor
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public decimal Rating { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset UpdatedAt { get; private set; }

        private Contractor() { }

        public Contractor(string name, decimal rating)
        {
            if (rating < 0 || rating > 5)
                throw new ArgumentException("Rating must be between 0 and 5");

            Id = Guid.NewGuid();
            Name = name;
            Rating = rating;
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public void UpdateName(string name)
        {
            Name = name;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public void UpdateRating(decimal rating)
        {
            if (rating < 0 || rating > 5)
                throw new ArgumentException("Rating must be between 0 and 5");

            Rating = rating;
            UpdatedAt = DateTimeOffset.UtcNow;
        }
    }

}
