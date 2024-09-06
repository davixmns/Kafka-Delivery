using System.Text.Json;
using Confluent.Kafka;

namespace KafkaDelivery.Infra.Services;

public class KafkaService : IKafkaService
{
    private readonly IProducer<string, string> _producer;
    
    public KafkaService(ProducerConfig config)
    {
        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task<DeliveryResult<string, string>> PublishMessageToTopicAsync<T>(T message, string topicName)
    {
        var formattedMessage = new Message<string, string>
        {
            Value = JsonSerializer.Serialize(message)
        };
        
        var deliveryResult = await _producer.ProduceAsync(topicName, formattedMessage);

        return deliveryResult;
    }
}