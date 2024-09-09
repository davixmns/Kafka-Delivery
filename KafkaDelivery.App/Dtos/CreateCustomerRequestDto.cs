namespace KafkaDelivery.App.Dtos;

public class CreateCustomerRequestDto
{
    public string Name { get; set; } 
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}