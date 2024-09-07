namespace KafkaDelivery.Domain.ValueObjects;

public enum OrderStatus
{
    PaymentPending,
    Paid,
    OnDelivery,
    Delivered,
    Canceled,
}