using KafkaDelivery.App.Commands;
using KafkaDelivery.App.Services;
using KafkaDelivery.App.Wrappers;
using KafkaDelivery.Domain.Entities;
using MediatR;

namespace KafkaDelivery.App.Handlers;

public class PayOrderCommandHandler : IRequestHandler<PayOrderCommand, AppResult<Order>>
{
    private readonly IOrderService _orderService;
    
    public PayOrderCommandHandler(IOrderService orderService)
    {
        _orderService = orderService;
    }
    
    public async Task<AppResult<Order>> Handle(PayOrderCommand request, CancellationToken cancellationToken)
    {
        return await _orderService.PayOrderAsync(request.Order);
    }
}