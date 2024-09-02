using System.Text.Json;
using Confluent.Kafka;

namespace KafkaDelivery.Infra.Services;

public class BrokerService : IBrokerService
{
    private readonly ProducerConfig _config;
    
    public BrokerService(ProducerConfig config)
    {
        _config = config;
    }
    
    public async Task<DeliveryResult<Null, string>> PublishMessageToTopicAsync<T>(T message, string topicName)
    {
        var formattedMessage = new Message<Null, string>
        {
            Value = JsonSerializer.Serialize(message)
        };

        using var producer = new ProducerBuilder<Null, string>(_config).Build();
        
        var deliveryResult = await producer.ProduceAsync(topicName, formattedMessage);
        
        return deliveryResult;
    }
}