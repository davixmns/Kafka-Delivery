namespace KafkaDelivery.Notifier.Services;

public class NotifierService : INotifierService
{
    public void NotifyUserOrderStatus(string name, string email, string orderStatus)
    {
        Console.WriteLine($"Hello {name}, your order status is {orderStatus}");
    }
}