using KafkaDelivery.Domain.Entities;

namespace KafkaDelivery.App.Dtos;

public class CreateOrderRequestDto
{
    public int CustomerId { get; set; }
    public IList<Product> Products { get; set; }
}