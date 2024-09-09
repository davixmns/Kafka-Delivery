using KafkaDelivery.App.Commands;
using KafkaDelivery.App.Wrappers;
using KafkaDelivery.Infra.Repositories;
using KafkaDelivery.Infra.Services;
using KafkaDelivery.Infra.Utils;
using KafkaDelivery.Domain.Entities;
using MediatR;

namespace KafkaDelivery.App.Handlers;

public class PayOrderCommandHandler : IRequestHandler<PayOrderCommand, AppResult<Order>>
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IKafkaService _kafkaService;
    
    public PayOrderCommandHandler(IRepository<Order> orderRepository, IKafkaService kafkaService)
    {
        _orderRepository = orderRepository;
        _kafkaService = kafkaService;
    }
    
    public async Task<AppResult<Order>> Handle(PayOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetAsync(o => o.Id == request.OrderId);
        
        var payResult = order!.Pay();
        
        if(!payResult.IsSuccess)
            return AppResult<Order>.Failure(payResult.ErrorMessage);
        
        await _orderRepository.UpdateAsync(order);
        
        await _kafkaService.PublishMessageToTopicAsync(
            message: order,
            topicName: KafkaTopics.OrdersPaid
        );
        
        return AppResult<Order>.Success(order);
    }
}