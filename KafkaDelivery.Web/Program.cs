
using Confluent.Kafka;
using KafkaDelivery.App.Services;
using KafkaDelivery.Infra.Repositories;
using KafkaDelivery.Infra.Services;

var builder = WebApplication.CreateBuilder(args);

var producerConfig = new ProducerConfig
{
    BootstrapServers = builder.Configuration["Kafka:BootstrapServers"],
    Acks = Acks.Leader,
};

builder.Services.AddSingleton(producerConfig);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IKafkaService, KafkaService>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();