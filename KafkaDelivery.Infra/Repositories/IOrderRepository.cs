using KafkaDelivery.Domain.Entities;

namespace KafkaDelivery.Infra.Repositories;

public interface IOrderRepository
{
    IList<Order> GetAll();
    IList<Order> GetOrdersByStatus(OrderStatus status);
    Order? GetById(string orderId);
    void Save(Order order);
    void Update(Order order);
    void Delete(string orderId);
}