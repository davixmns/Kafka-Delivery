using KafkaDelivery.Notifier.Utils;

namespace KafkaDelivery.Notifier.Services;

public interface INotifierService
{
    void NotifyUserOrderStatus(string name, string email, int orderId, OrderStatus orderStatus);
}