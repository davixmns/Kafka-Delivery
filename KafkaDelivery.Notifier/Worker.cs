using System.Text.Json;
using Confluent.Kafka;
using KafkaDelivery.Domain.Entities;
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
        while (!stoppingToken.IsCancellationRequested)
        {
            CancellationTokenSource cts = new();
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true; // prevent the process from terminating.
                cts.Cancel();
            };

            _logger.LogInformation("Order netifier is running at: {time}", DateTimeOffset.UtcNow);

            using var consumer = new ConsumerBuilder<Ignore, string>(_kafkaConsumerConfig).Build();

            consumer.Subscribe(KafkaTopics.Orders);

            try
            {
                while (true)
                {
                    var consumeResult = consumer.Consume(cts.Token);

                    Console.WriteLine("ConsumeResult: " + consumeResult.Message.Value);

                    using var jsonDoc = JsonDocument.Parse(consumeResult.Message.Value);
                    var root = jsonDoc.RootElement;

                    var customerEmail = root.GetProperty("Customer").GetProperty("Email").GetString()!;

                    _notifierService.Notify(customerEmail);

                    consumer.Commit(consumeResult);
                }
            }
            catch (Exception e)
            {
                if(e is OperationCanceledException)
                    consumer.Close();
                else if (e is ConsumeException)
                    _logger.LogError("Error consuming message: {error}", e.Message);
                else
                    _logger.LogError("Error: {error}", e.Message);
            }
        }
    }
}