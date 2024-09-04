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
}