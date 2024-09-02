using KafkaDelivery.Domain.Entities;

namespace KafkaDelivery.Infra.Repositories;

public class OrderRepository: IOrderRepository
{
    private static IList<Order> _orders = [];
    
    public IList<Order> GetAll()
    {
        return _orders;
    }

    public Order? GetById(string orderId)
    {
        return _orders.FirstOrDefault(x => x.OrderId == orderId);
    }

    public void Save(Order order)
    {
        _orders.Add(order);
    }

    public void Update(Order order)
    {
        var findedOrder = _orders.FirstOrDefault(x => x.OrderId == order.OrderId);
        if (findedOrder != null)
        {
            findedOrder = order;
        }
    }

    public void Delete(string orderId)
    {
        var findedOrder = _orders.FirstOrDefault(x => x.OrderId == orderId);
        if (findedOrder != null)
        {
            _orders.Remove(findedOrder);
        }
    }
    
    public IList<Order> GetOrdersByStatus(OrderStatus status)
    {
        return _orders.Where(x => x.Status == status).ToList();
    }
}