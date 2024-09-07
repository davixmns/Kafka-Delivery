using KafkaDelivery.App.Dtos;
using KafkaDelivery.App.Wrappers;
using KafkaDelivery.Domain.Entities;
using MediatR;

namespace KafkaDelivery.App.Commands;

public class PayOrderCommand : IRequest<AppResult<Order>>
{
    public int OrderId { get; set; }
    public string PaymentMethod { get; set; }

    public PayOrderCommand(int orderId, string paymentMethod) 
    {
        OrderId = orderId;
        PaymentMethod = paymentMethod;
    }
}