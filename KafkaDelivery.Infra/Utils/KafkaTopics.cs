namespace KafkaDelivery.Infra.Utils;

public static class KafkaTopics
{
    public const string OrdersPaymentPending = "orders-payment-pending";
    public const string OrdersPaid = "orders-paid";
    public const string OrdersOnDelivery = "orders-on-delivery";
    public const string OrdersDelivered = "orders-delivered";
    public const string OrdersCanceled = "orders-canceled";
}