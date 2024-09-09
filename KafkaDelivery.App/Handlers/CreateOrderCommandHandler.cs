using KafkaDelivery.App.Commands;
using KafkaDelivery.App.Wrappers;
using KafkaDelivery.Domain.Entities;
using KafkaDelivery.Infra.Repositories;
using KafkaDelivery.Infra.Services;
using KafkaDelivery.Infra.Utils;
using MediatR;

namespace KafkaDelivery.App.Handlers;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, AppResult<Order>>
{
    private readonly IKafkaService _kafkaService;
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<Customer> _customerRepository;
    
    public CreateOrderCommandHandler(IKafkaService kafkaService, IRepository<Order> orderRepository, IRepository<Customer> customerRepository)
    {
        _kafkaService = kafkaService;
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
    }
    
    public async Task<AppResult<Order>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetAsync(c => c.Id == request.CustomerId);
        
        var order = new Order(request.Products, customer!);
        
        await _orderRepository.SaveAsync(order);
        
        await _kafkaService.PublishMessageToTopicAsync(
            message: order,
            topicName: KafkaTopics.OrdersPaymentPending 
        );
        
        return AppResult<Order>.Success(order);
    }
}