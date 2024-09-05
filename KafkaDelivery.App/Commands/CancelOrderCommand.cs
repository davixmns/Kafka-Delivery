using KafkaDelivery.App.Wrappers;
using KafkaDelivery.Domain.Entities;
using MediatR;

namespace KafkaDelivery.App.Commands;

public class CancelOrderCommand : IRequest<AppResult<Order>>
{
    public int OrderId { get; set; }
    public Order Order { get; set; }
    
    public CancelOrderCommand(int orderId)
    {
        OrderId = orderId;
    }
}