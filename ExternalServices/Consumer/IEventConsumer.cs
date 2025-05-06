using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExternalServices.Common;

namespace ExternalServices.Consumer
{
    public interface IEventConsumer
    {
        Task<CheckAvailabilityResponseEvent> Consume(string topic, string key);
    }
}
