using KafkaDelivery.App.Commands;
using KafkaDelivery.Infra.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using KafkaDelivery.Domain.Entities;

namespace KafkaDelivery.App.Handlers;

public class GetCustomerOrdersQueryHandler : IRequestHandler<GetCustomerOrdersQuery, IEnumerable<Order>>
{
    private readonly IRepository<Order> _orderRepository;
    
    public GetCustomerOrdersQueryHandler(IRepository<Order> orderRepository)
    {
        _orderRepository = orderRepository;
    }
    
    public async Task<IEnumerable<Order>> Handle(GetCustomerOrdersQuery request, CancellationToken cancellationToken)
    {
        var customerOrders = await _orderRepository.GetAll()
            .Where(o => o.CustomerId == request.CustomerId)
            .Include(o => o.Products)
            .ToListAsync(cancellationToken);
        
        return customerOrders;
    }
}