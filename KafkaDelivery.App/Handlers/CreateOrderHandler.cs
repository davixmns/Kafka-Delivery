using KafkaDelivery.App.Commands;
using KafkaDelivery.App.Services;
using KafkaDelivery.App.Wrappers;
using KafkaDelivery.Domain.Entities;
using MediatR;

namespace KafkaDelivery.App.Handlers;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, AppResult<Order>>
{
    private readonly IOrderService _orderService;
    
    public CreateOrderHandler(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task<AppResult<Order>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var newOrder = new Order(request.Items, request.Customer);
        return await _orderService.CreateOrderAsync(newOrder);
    }
}