namespace KafkaDelivery.Domain.Entities;

public enum OrderStatus
{
    PaymentPending,
    Paid,
    OnDelivery,
    Devlivered,
    Canceled,
}