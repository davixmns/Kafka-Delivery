using Confluent.Kafka;

namespace KafkaDelivery.Infra.Services;

public interface IBrokerService
{
    Task<DeliveryResult<Null, string>> PublishMessageToTopicAsync<T>(T message, string topicName);
}