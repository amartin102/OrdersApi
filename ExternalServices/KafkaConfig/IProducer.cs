using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExternalServices.Common;

namespace ExternalServices.KafkaConfig
{
    public interface IProducer
    {
        Task SendAsync<T>(string topic, T customerEvent);

       // Task SendAsync(string topic, CreateOrderEvent customerEvent);
    }
}
