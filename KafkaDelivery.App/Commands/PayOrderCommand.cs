using KafkaDelivery.App.Dtos;
using KafkaDelivery.App.Wrappers;
using KafkaDelivery.Domain.Entities;
using MediatR;

namespace KafkaDelivery.App.Commands;

public class PayOrderCommand : IRequest<AppResult<Order>>
{
    public string OrderId { get; set; }
    public string PaymentMethod { get; set; }
    public Order Order { get; set; }

    public PayOrderCommand(PayOrderRequestDto payOrderRequestDto, string orderId)
    {
        OrderId = orderId;
        PaymentMethod = payOrderRequestDto.PaymentMethod;
    }
}