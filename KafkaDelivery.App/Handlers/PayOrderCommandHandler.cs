using KafkaDelivery.App.Commands;
using KafkaDelivery.App.Services;
using KafkaDelivery.App.Wrappers;
using KafkaDelivery.Domain.Entities;
using KafkaDelivery.Infra.Repositories;
using MediatR;

namespace KafkaDelivery.App.Handlers;

public class PayOrderCommandHandler : IRequestHandler<PayOrderCommand, AppResult<Order>>
{
    private readonly IOrderService _orderService;
    private readonly IRepository<Order> _orderRepository;
    
    public PayOrderCommandHandler(IOrderService orderService, IRepository<Order> orderRepository)
    {
        _orderService = orderService;
        _orderRepository = orderRepository;
    }
    
    public async Task<AppResult<Order>> Handle(PayOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetAsync(o => o.Id == request.OrderId);
        return await _orderService.PayOrderAsync(order!);
    }
}