using System.Text.Json;
using Confluent.Kafka;
using KafkaDelivery.Notifier.Services;
using KafkaDelivery.Notifier.Utils;

namespace KafkaDelivery.Notifier;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly INotifierService _notifierService;
    private readonly ConsumerConfig _kafkaConsumerConfig;

    public Worker(ILogger<Worker> logger, INotifierService notifierService, ConsumerConfig kafkaConsumerConfig)
    {
        _logger = logger;
        _notifierService = notifierService;
        _kafkaConsumerConfig = kafkaConsumerConfig;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Order notifier is running at: {time}", DateTimeOffset.UtcNow);

        using var consumer = new ConsumerBuilder<Ignore, string>(_kafkaConsumerConfig).Build();

        consumer.Subscribe(new List<string>
        {
            KafkaTopics.OrdersPaymentPending,
            KafkaTopics.OrdersPaid,
            KafkaTopics.OrdersOnDelivery,
            KafkaTopics.OrdersDelivered,
            KafkaTopics.OrdersCanceled
        });

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var consumeResult = consumer.Consume(stoppingToken);
                using var jsonDoc = JsonDocument.Parse(consumeResult.Message.Value);
                var root = jsonDoc.RootElement;
                
                var customer = root.GetProperty("Customer");
                var customerEmail= customer.GetProperty("Email").GetString()!;
                var orderStatus = root.GetProperty("Status").GetInt32();
                var orderId = root.GetProperty("Id").GetInt32();
                var userName = customer.GetProperty("Name").GetString()!;
                
                _notifierService.NotifyUserOrderStatus(userName, customerEmail, orderId, (OrderStatus) orderStatus);
                consumer.Commit();
            }
        }
        catch (Exception e)
        {
            switch (e)
            {
                case OperationCanceledException:
                    consumer.Close();
                    break;
                case ConsumeException:
                    _logger.LogError("Error consuming message: {error}", e.Message);
                    break;
                default:
                    _logger.LogError("Error: {error}", e.Message);
                    break;
            }
        }
    }
}