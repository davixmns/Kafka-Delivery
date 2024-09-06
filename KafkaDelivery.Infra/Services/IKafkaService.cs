using Confluent.Kafka;

namespace KafkaDelivery.Infra.Services;

public interface IKafkaService
{
    Task<DeliveryResult<string, string>> PublishMessageToTopicAsync<T>(T message, string topicName);
}