using Confluent.Kafka;
using KafkaDelivery.App.Wrappers;
using KafkaDelivery.Domain.Entities;
using KafkaDelivery.Infra.Repositories;
using KafkaDelivery.Infra.Services;
using KafkaDelivery.Infra.Utils;

namespace KafkaDelivery.App.Services;

public class OrderService : IOrderService
{
    private readonly IKafkaService _kafkaService;
    private readonly IOrderRepository _orderRepository;

    public OrderService(IKafkaService kafkaService, IOrderRepository orderRepository)
    {
        _kafkaService = kafkaService;
        _orderRepository = orderRepository;
    }

    public async Task<AppResult<Order>> CreateOrderAsync(Order order)
    {
        _orderRepository.Save(order);
        
        await _kafkaService.PublishMessageToTopicAsync(
            message: order,
            topicName: KafkaTopics.Orders, 
            partition: (int)OrderStatus.PaymentPending
        );
        
        return AppResult<Order>.Success(order);
    }

    public async Task<AppResult<Order>> PayOrderAsync(Order order)
    {
        var payResult = order.Pay();
        
        if(!payResult.IsSuccess)
            return AppResult<Order>.Failure(payResult.ErrorMessage);
        
        _orderRepository.Update(order);
        
        await _kafkaService.PublishMessageToTopicAsync(
            message: order,
            topicName: KafkaTopics.Orders, 
            partition: (int)OrderStatus.Paid
        );
        
        return AppResult<Order>.Success(order);
    }

    public async Task<AppResult<Order>> CancelOrderAsync(Order order)
    {
        var cancelResult = order.Cancel();
        
        if(!cancelResult.IsSuccess)
            return AppResult<Order>.Failure(cancelResult.ErrorMessage);
        
        _orderRepository.Update(order);
        
        await _kafkaService.PublishMessageToTopicAsync(
            message: order,
            topicName: KafkaTopics.Orders, 
            partition: (int)OrderStatus.Canceled
        );
        
        return AppResult<Order>.Success(order);
    }
}