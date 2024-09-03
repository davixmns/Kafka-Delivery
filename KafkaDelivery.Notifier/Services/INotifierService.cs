namespace KafkaDelivery.Notifier.Services;

public interface INotifierService
{
    void Notify(string email);
}