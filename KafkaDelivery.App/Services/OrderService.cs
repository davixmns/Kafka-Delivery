using KafkaDelivery.App.Wrappers;
using KafkaDelivery.Domain.Entities;
using KafkaDelivery.Infra.Repositories;
using KafkaDelivery.Infra.Services;
using KafkaDelivery.Infra.Utils;

namespace KafkaDelivery.App.Services;

public class OrderService : IOrderService
{
    private readonly IKafkaService _kafkaService;
    private readonly IRepository<Order> _orderRepository;

    public OrderService(IKafkaService kafkaService, IRepository<Order> orderRepository)
    {
        _kafkaService = kafkaService;
        _orderRepository = orderRepository;
    }

    public async Task<AppResult<Order>> CreateOrderAsync(Order order)
    {
        await _orderRepository.SaveAsync(order);
        
        await _kafkaService.PublishMessageToTopicAsync(
            message: order,
            topicName: KafkaTopics.OrdersPaymentPending 
        );
        
        return AppResult<Order>.Success(order);
    }

    public async Task<AppResult<Order>> PayOrderAsync(Order order)
    {
        var payResult = order.Pay();
        
        if(!payResult.IsSuccess)
            return AppResult<Order>.Failure(payResult.ErrorMessage);
        
        await _orderRepository.UpdateAsync(order);
        
        await _kafkaService.PublishMessageToTopicAsync(
            message: order,
            topicName: KafkaTopics.OrdersPaid
        );
        
        return AppResult<Order>.Success(order);
    }

    public async Task<AppResult<Order>> CancelOrderAsync(Order order)
    {
        var cancelResult = order.Cancel();
        
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