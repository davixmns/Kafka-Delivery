namespace KafkaDelivery.Notifier.Services;

public interface INotifierService
{
    void NotifyUserOrderStatus(string name, string email, string orderStatus);
}