using KafkaDelivery.App.Dtos;
using KafkaDelivery.App.Wrappers;
using KafkaDelivery.Domain.Entities;
using MediatR;

namespace KafkaDelivery.App.Commands;

public class CreateOrderCommand : IRequest<AppResult<Order>>
{
    public string CustomerId { get; set; }
    public Customer Customer { get; set; }
    public IList<string> Items { get; set; }
    
    public CreateOrderCommand(CreateOrderRequestDto orderRequestDto)
    {
        CustomerId = orderRequestDto.CustomerId;
        Items = orderRequestDto.Items;
    }
}