using Confluent.Kafka;
using FluentValidation;
using KafkaDelivery.App.Commands;
using KafkaDelivery.App.PipelineBehaviors;
using KafkaDelivery.App.Services;
using KafkaDelivery.App.Validators;
using KafkaDelivery.Infra.Context;
using KafkaDelivery.Infra.Repositories;
using KafkaDelivery.Infra.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DbContext
var mysqlConnection = builder.Configuration["MySQL:ConnectionString"];
builder.Services.AddDbContext<DeliveryDbContext>(options =>
    options.UseMySql(mysqlConnection, ServerVersion.AutoDetect(mysqlConnection)));

// Repositório genérico
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateOrderCommand>());
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderCommandValidator>();

// Services
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IKafkaService, KafkaService>(ks =>
{
    var producerConfig = new ProducerConfig
    {
        BootstrapServers = builder.Configuration["Kafka:BootstrapServers"],
        Acks = Acks.Leader,
    };
    return new KafkaService(producerConfig);
});

// Desativando a validação automática do ModelState
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();