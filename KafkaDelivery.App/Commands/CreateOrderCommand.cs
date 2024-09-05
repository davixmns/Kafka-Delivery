using KafkaDelivery.App.Dtos;
using KafkaDelivery.App.Wrappers;
using KafkaDelivery.Domain.Entities;
using MediatR;

namespace KafkaDelivery.App.Commands;

public class CreateOrderCommand : IRequest<AppResult<Order>>
{
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    public IList<Product> Products { get; set; }
    
    public CreateOrderCommand(CreateOrderRequestDto orderRequestDto)
    {
        CustomerId = orderRequestDto.CustomerId;
        Products = orderRequestDto.Products;
    }
}