using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalServices.Common
{
    public class CreatedOrderEvent
    {
        public CreatedOrderEvent(Guid guid, string name, int orderId)
        {
            Guid = guid;
            Name = name;
            OrderId = orderId;
        }

        public Guid Guid { get; set; }
        public string Name { get; set; }
        public int OrderId { get; set; }
    }
}
