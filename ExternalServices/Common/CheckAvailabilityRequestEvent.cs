using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalServices.Common
{
    public class CheckAvailabilityRequestEvent
    {
        public CheckAvailabilityRequestEvent(Guid guid, int orderId, List<int> recipes)
        {
            Guid = guid;
            OrderId = orderId;
            Recipes = recipes;
        }

        public Guid Guid { get; set; }
        //public bool Availability { get; set; }
        public int OrderId { get; set; }

        public List<int> Recipes { get; set; }

    }
}
