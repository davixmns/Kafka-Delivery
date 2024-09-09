
using KafkaDelivery.App.Commands;
using KafkaDelivery.App.Wrappers;
using KafkaDelivery.Infra.Repositories;
using KafkaDelivery.Infra.Services;
using KafkaDelivery.Infra.Utils;
using MediatR;
using KafkaDelivery.Domain.Entities;

namespace KafkaDelivery.App.Handlers;

public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, AppResult<Order>>
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IKafkaService _kafkaService;
    
    public CancelOrderCommandHandler(IRepository<Order> orderRepository, IKafkaService kafkaService)
    {
        _orderRepository = orderRepository;
        _kafkaService = kafkaService;
    }
    
    public async Task<AppResult<Order>> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetAsync(
            o => o.Id == request.OrderId,
            o => o.Customer
        );
        
        var cancelResult = order!.Cancel();
        
        if(!cancelResult.IsSuccess)
            return AppResult<Order>.Failure(cancelResult.ErrorMessage);
        
        await _orderRepository.UpdateAsync(order);
        
        await _kafkaService.PublishMessageToTopicAsync(
            message: order,
            topicName: KafkaTopics.OrdersCanceled
        );
        
        return AppResult<Order>.Success(order);
    }
}