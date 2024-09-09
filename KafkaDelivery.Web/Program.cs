using Confluent.Kafka;
using FluentValidation;
using FluentValidation.AspNetCore;
using KafkaDelivery.App.Commands;
using KafkaDelivery.App.Validators;
using KafkaDelivery.Infra.Context;
using KafkaDelivery.Infra.Repositories;
using KafkaDelivery.Infra.Services;
using KafkaDelivery.Infra.Utils;
using Microsoft.EntityFrameworkCore;
using Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// DbContext
var mysqlConnection = builder.Configuration["MySQL:ConnectionString"];
builder.Services.AddDbContext<DeliveryDbContext>(options =>
    options.UseMySql(mysqlConnection, ServerVersion.AutoDetect(mysqlConnection)));

// Repositório genérico
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateOrderCommand>());
// builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderRequestDtoValidator>();
builder.Services.AddFluentValidationAutoValidation();

// Kafka Services
builder.Services.AddSingleton<IKafkaAdminService, KafkaAdminService>(ks =>
{
    var adminConfig = new AdminClientConfig
    {
        BootstrapServers = builder.Configuration["Kafka:BootstrapServers"],
    };
    return new KafkaAdminService(adminConfig, ks.GetRequiredService<ILogger<KafkaAdminService>>());
});

builder.Services.AddScoped<IKafkaService, KafkaService>(ks =>
{
    var producerConfig = new ProducerConfig
    {
        BootstrapServers = builder.Configuration["Kafka:BootstrapServers"],
        Acks = Acks.Leader,
    };
    return new KafkaService(producerConfig);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

// Middleware de exceção global
app.UseMiddleware<GlobalExceptionMiddleware>();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var kafkaTopics = new List<string>
{
    KafkaTopics.OrdersPaymentPending,
    KafkaTopics.OrdersPaid,
    KafkaTopics.OrdersOnDelivery,
    KafkaTopics.OrdersDelivered,
    KafkaTopics.OrdersCanceled,
};

var kafkaAdminService = app.Services.GetRequiredService<IKafkaAdminService>();

await kafkaAdminService.CreateTopicsAsync(
    topicNames: kafkaTopics,
    numPartitions: 3,
    replicationFactor: 1
);

app.Run();