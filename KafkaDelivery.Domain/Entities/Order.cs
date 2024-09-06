using KafkaDelivery.Domain.Wrappers;

namespace KafkaDelivery.Domain.Entities;

public class Order : BaseEntity
{
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    public IList<Product> Products { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Order(IList<Product> products, Customer customer)
    {
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        Status = OrderStatus.PaymentPending;
        Products = products;
        Customer = customer ?? throw new ArgumentNullException(nameof(customer));
        CustomerId = customer.Id;
    }
    
    public Order()
    {
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