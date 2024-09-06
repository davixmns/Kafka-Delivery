using KafkaDelivery.Domain.Entities;
using MediatR;

namespace KafkaDelivery.App.Commands;

public class GetCustomerOrdersQuery : IRequest<IEnumerable<Order>>
{
    public int CustomerId { get; set; }
    
    public GetCustomerOrdersQuery(int customerId)
    {
        CustomerId = customerId;
    }
}