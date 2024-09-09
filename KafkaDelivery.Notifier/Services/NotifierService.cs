using KafkaDelivery.Notifier.Utils;

namespace KafkaDelivery.Notifier.Services;

public class NotifierService : INotifierService
{
    public void NotifyUserOrderStatus(string name, string email, int orderId, OrderStatus status)
    {
        switch (status)
        {
            case OrderStatus.PaymentPending:
                Console.WriteLine($"Dear {name}, your order {orderId} is pending payment.");
                break;
            
            case OrderStatus.Paid:
                Console.WriteLine($"Dear {name}, your order {orderId} is paid.");
                break;
            
            case OrderStatus.OnDelivery:
                Console.WriteLine($"Dear {name}, your order {orderId} is on delivery.");
                break;
            
            case OrderStatus.Delivered:
                Console.WriteLine($"Dear {name}, your order {orderId} is delivered.");
                break;
            
            case OrderStatus.Canceled:
                Console.WriteLine($"Dear {name}, your order {orderId} is canceled.");
                break;
            
            default:
                Console.WriteLine($"Dear {name}, your order {orderId} is in an unknown status.");
                break;
            
        }
    }
}