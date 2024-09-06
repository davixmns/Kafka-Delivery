using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Logging;

namespace KafkaDelivery.Infra.Services;

public class KafkaAdminService : IKafkaAdminService
{
    private readonly AdminClientConfig _adminClientConfig;
    private readonly ILogger<KafkaAdminService> _logger;

    public KafkaAdminService(AdminClientConfig adminClientConfig, ILogger<KafkaAdminService> logger)
    {
        _adminClientConfig = adminClientConfig;
        _logger = logger;
        VerifyConnection();
    }

    private void VerifyConnection()
    {
        using var adminClient = new AdminClientBuilder(_adminClientConfig).Build();

        try
        {
            adminClient.GetMetadata(TimeSpan.FromSeconds(10));
            _logger.LogInformation("Kafka connection successful!");
        }
        catch (Exception e)
        {
            _logger.LogError($"Kafka connection Failed: {e.Message}");
            Environment.Exit(1);
        }
    }

    public async Task CreateTopicsAsync(IEnumerable<string> topicNames,
        int numPartitions = 1, short replicationFactor = 1)
    {
        using var adminClient = new AdminClientBuilder(_adminClientConfig).Build();

        var existingTopics = adminClient.GetMetadata(TimeSpan.FromSeconds(10))
            .Topics
            .Select(t => t.Topic)
            .ToHashSet();

        var topicsToCreate = topicNames
            .Where(topic => !existingTopics.Contains(topic))
            .Select(topic => new TopicSpecification
            {
                Name = topic,
                NumPartitions = numPartitions,
                ReplicationFactor = replicationFactor
            })
            .ToList();

        if (topicsToCreate.Any())
        {
            try
            {
                Console.WriteLine("Creating Kafka Topics...");
                await adminClient.CreateTopicsAsync(topicsToCreate);
                Console.WriteLine("Kafka Topics created successfully!");
            }
            catch (CreateTopicsException e)
            {
                foreach (var result in e.Results.Where(result => result.Error.Code is not ErrorCode.TopicAlreadyExists))
                {
                    Console.WriteLine($"An error occurred creating topic {result.Topic}: {result.Error.Reason}");
                }
            }
        }
    }
}