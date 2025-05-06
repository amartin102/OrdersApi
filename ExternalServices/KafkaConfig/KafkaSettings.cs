using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalServices.KafkaConfig
{
    public class KafkaSettings
    {
        public string Hostname { get; set; } = string.Empty;
        public string Port { get; set; } = string.Empty;
        public string CheckAvailabilityRequestTopic { get; set; } = string.Empty;
        public string CheckAvailabilityResponseTopic { get; set; } = string.Empty;
        public string CreatedOrderTopic { get; set; } = string.Empty;
    }
}
