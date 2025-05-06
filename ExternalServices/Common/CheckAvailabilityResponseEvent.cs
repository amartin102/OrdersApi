using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalServices.Common
{
    public class CheckAvailabilityResponseEvent
    {
        public CheckAvailabilityResponseEvent(Guid guid, int orderId, bool isAvailable)
        {
            Guid = guid;
            OrderId = orderId;
            IsAvailable = isAvailable;
        }

        public Guid Guid { get; set; }
        public int OrderId { get; set; }
        public bool IsAvailable { get; set; }

    }
}
