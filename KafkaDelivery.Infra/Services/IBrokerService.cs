using Confluent.Kafka;

namespace KafkaDelivery.Infra.Services;

public interface IBrokerService
{
    Task<DeliveryResult<string, string>> PublishMessageToTopicAsync<T>(T message, string topicName, int partition);
}