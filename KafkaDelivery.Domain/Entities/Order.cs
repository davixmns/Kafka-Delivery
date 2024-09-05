using KafkaDelivery.Domain.Wrappers;

namespace KafkaDelivery.Domain.Entities;

public class Order
{
    public string OrderId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public OrderStatus Status { get; set; }
    public IList<string> Items { get; set; }
    public Customer Customer { get; set; }

    public Order(IList<string> items, Customer customer)
    {
        OrderId = Guid.NewGuid().ToString()[..8];
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        Status = OrderStatus.PaymentPending;
        Items = items;
        Customer = customer;
    }
    
    public DomainResult Pay()
    {
        if (Status is not OrderStatus.PaymentPending)
            return DomainResult.Failure("Order is already paid");
        
        Status = OrderStatus.Paid; 
        UpdatedAt = DateTime.UtcNow;
        
        return DomainResult.Success();
    }

    public DomainResult Cancel()
    {
        if(Status is not OrderStatus.PaymentPending)
            return DomainResult.Failure("The order cannot be canceled");

        Status = OrderStatus.Canceled;
        UpdatedAt = DateTime.UtcNow;

        return DomainResult.Success();
    }
}