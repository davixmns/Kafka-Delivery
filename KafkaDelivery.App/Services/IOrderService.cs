using KafkaDelivery.App.Wrappers;
using KafkaDelivery.Domain.Entities;

namespace KafkaDelivery.App.Services;

public interface IOrderService
{
    Task<AppResult<Order>> CreateOrderAsync(Order order);
    Task<AppResult<Order>> PayOrderAsync(Order order);
    Task<AppResult<Order>> CancelOrderAsync(Order order);
}