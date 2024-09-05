using System.Text.Json.Serialization;

namespace KafkaDelivery.Domain.Entities;

public class Product : BaseEntity
{
    public int OrderId { get; set; }
    [JsonIgnore]
    public Order Order { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Price { get; set; }
}