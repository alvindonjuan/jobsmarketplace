using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsMarketplace.Domain.Enums
{
    public enum JobStatus : short
    {
        Open = 0,
        InProgress = 1,
        Completed = 2,
        Cancelled = 3
    }
}
