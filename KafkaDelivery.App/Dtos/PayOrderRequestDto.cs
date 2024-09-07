namespace KafkaDelivery.App.Dtos;

public class PayOrderRequestDto
{
    public int OrderId { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
}