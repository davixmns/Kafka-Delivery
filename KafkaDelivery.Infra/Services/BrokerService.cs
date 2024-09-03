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

    public async Task<DeliveryResult<string, string>> PublishMessageToTopicAsync<T>(T message, string topicName, int partition)
    {
        var formattedMessage = new Message<string, string>
        {
            Value = JsonSerializer.Serialize(message)
        };

        using var producer = new ProducerBuilder<string, string>(_config).Build();

        var deliveryResult = await producer.ProduceAsync(
            new TopicPartition(topicName, new Partition(partition)), formattedMessage);

        return deliveryResult;
    }
}