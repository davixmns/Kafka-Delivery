namespace KafkaDelivery.Infra.Services;

public interface IKafkaAdminService
{
    Task CreateTopicsAsync(IEnumerable<string> topicNames, int numPartitions = 1, short replicationFactor = 1);
}