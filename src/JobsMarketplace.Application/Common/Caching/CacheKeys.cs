using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsMarketplace.Application.Common.Caching
{
    public static class CacheKeys
    {
        public static string Customer(Guid id) => $"customer:{id}";

        public static string Contractor(Guid id)  => $"contractor:{id}";

        public static string Job(Guid id) => $"job:{id}";

        public static string JobOffer(Guid id) => $"joboffer:{id}";
    }
}
