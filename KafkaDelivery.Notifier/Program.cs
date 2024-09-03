using Confluent.Kafka;
using KafkaDelivery.Notifier;
using KafkaDelivery.Notifier.Services;

var builder = Host.CreateApplicationBuilder(args);

var consumerConfig = new ConsumerConfig
{
    BootstrapServers = builder.Configuration["Kafka:BootstrapServers"],
    GroupId = builder.Configuration["Kafka:NotifierConsumerGroupName"],
    AutoOffsetReset = AutoOffsetReset.Earliest
};

builder.Services.AddSingleton(consumerConfig);

builder.Services.AddSingleton<INotifierService, NotifierService>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();