using System.Text.Json;
using Confluent.Kafka;

namespace KafkaDelivery.Infra.Services;

public class KafkaService : IKafkaService
{
    private readonly ProducerConfig _config;
    private readonly IProducer<string, string> _producer;

    public KafkaService(ProducerConfig config)
    {
        _config = config;
        _producer = new ProducerBuilder<string, string>(_config).Build();
    }

    public async Task<DeliveryResult<string, string>> PublishMessageToTopicAsync<T>(T message, string topicName, int partition)
    {
        var formattedMessage = new Message<string, string>
        {
            Value = JsonSerializer.Serialize(message)
        };
        
        var deliveryResult = await _producer.ProduceAsync(
            new TopicPartition(topicName, new Partition(partition)), formattedMessage);

        return deliveryResult;
    }
}