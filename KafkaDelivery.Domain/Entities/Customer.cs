namespace KafkaDelivery.Domain.Entities;

public class Customer : BaseEntity
{
    public string CustomerId { get; set; }
    public string Name { get; set; } 
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    
    public Customer(string name, string email, string phoneNumber)
    {
        CustomerId = Guid.NewGuid().ToString()[..8];
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
    }
}