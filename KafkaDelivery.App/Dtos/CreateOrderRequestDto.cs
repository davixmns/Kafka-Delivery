namespace KafkaDelivery.App.Dtos;

public class CreateOrderRequestDto
{
    public string CustomerId { get; set; }
    public IList<string> Items { get; set; }
}