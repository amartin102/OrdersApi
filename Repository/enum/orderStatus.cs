using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Enums
{
    public enum OrderStatus
    {
        Pending = 1,
        RejectedByStock = 2,
        Confirmed = 3,
        Delivered = 4,
        Cancelled = 5
    }
}
