using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Confluent.Kafka;
using ExternalServices.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ExternalServices.KafkaConfig;
using static Confluent.Kafka.ConfigPropertyNames;

namespace ExternalServices.Consumer
{
    public class EventConsumer : IEventConsumer
    {
        private readonly ILogger<EventConsumer> _logger;
        private readonly ConsumerConfig _config;
        private readonly IServiceScopeFactory _serviceProvider;
        private readonly IProducer _producerBus;
        private readonly IOptions<KafkaSettings> _kafkaSettings;


        public EventConsumer(IOptions<ConsumerConfig> config, IServiceScopeFactory serviceProvider, ILogger<EventConsumer> logger, IProducer producer, IOptions<KafkaSettings> kafkaSettings)
        {
            _config = config.Value;
            _serviceProvider = serviceProvider;
            _logger = logger;
            _producerBus = producer;
            _kafkaSettings = kafkaSettings;
        }

        public async Task<CheckAvailabilityResponseEvent> Consume(string topic, string key)
        {
            using var consumer = new ConsumerBuilder<string, string>(_config)
                       .SetKeyDeserializer(Deserializers.Utf8)
                       .SetValueDeserializer(Deserializers.Utf8)
                       .Build();

            consumer.Subscribe(topic);

            while (true)
            {
                var consumeResult = consumer.Consume(TimeSpan.FromSeconds(3));
                if (consumeResult is null || consumeResult.Message is null) {
                    _logger.LogInformation($"No hay mensajes a leer");
                    continue;
                }

                var eventObject = (topic == "CheckAvailabilityResponse_Topic" ? JsonSerializer.Deserialize<CheckAvailabilityResponseEvent>(consumeResult.Message.Value,
                                                new JsonSerializerOptions { }) : null);

                if (eventObject is null)
                {
                    throw new ArgumentNullException("no se pudo procesar el mensaje");
                }

                if (!string.IsNullOrEmpty(key) && key == eventObject.Guid.ToString())
                {
                    Console.WriteLine($"Mensaje para mí: Key = {key}, Value = {eventObject.IsAvailable}");
                    // Procesar mensaje y romper bucle si ya obtuviste tu respuesta 
                   
                    consumer.Commit(consumeResult);
                    _logger.LogInformation($"Mensaje procesado: {consumeResult.Message.Value}");
                    //break;
                    return eventObject;
                }                      

            }

        }

    }
}
