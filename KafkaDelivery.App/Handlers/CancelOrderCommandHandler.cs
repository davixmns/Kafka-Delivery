using KafkaDelivery.App.Commands;
using KafkaDelivery.App.Services;
using KafkaDelivery.App.Wrappers;
using KafkaDelivery.Domain.Entities;
using MediatR;

namespace KafkaDelivery.App.Handlers;

public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, AppResult<Order>>
{
    private readonly IOrderService _orderService;
    
    public CancelOrderCommandHandler(IOrderService orderService)
    {
        _orderService = orderService;
    }
    
    public async Task<AppResult<Order>> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        return await _orderService.CancelOrderAsync(request.Order); 
    }
}