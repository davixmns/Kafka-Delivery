using KafkaDelivery.App.Commands;
using KafkaDelivery.App.Services;
using KafkaDelivery.App.Wrappers;
using KafkaDelivery.Domain.Entities;
using MediatR;

namespace KafkaDelivery.App.Handlers;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, AppResult<Order>>
{
    private readonly IOrderService _orderService;
    
    public CreateOrderCommandHandler(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task<AppResult<Order>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var newOrder = new Order(request.Products, request.Customer);
        return await _orderService.CreateOrderAsync(newOrder);
    }
}