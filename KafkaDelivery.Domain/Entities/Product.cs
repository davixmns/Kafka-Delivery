using System.Text.Json.Serialization;

namespace KafkaDelivery.Domain.Entities;

public class Product : BaseEntity
{
    public int OrderId { get; init; }
    public string Name { get; init; } = string.Empty;
    public double Price { get; init; }
}