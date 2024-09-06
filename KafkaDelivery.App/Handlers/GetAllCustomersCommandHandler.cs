using KafkaDelivery.App.Commands;
using KafkaDelivery.Domain.Entities;
using KafkaDelivery.Infra.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KafkaDelivery.App.Handlers;

public class GetAllCustomersCommandHandler : IRequestHandler<GetAllCustomersCommand, IEnumerable<Customer>>
{
    private readonly IRepository<Customer> _customerRepository;
    
    public GetAllCustomersCommandHandler(IRepository<Customer> customerRepository)
    {
        _customerRepository = customerRepository;
    }
    
    public async Task<IEnumerable<Customer>> Handle(GetAllCustomersCommand request, CancellationToken cancellationToken)
    {
        return await _customerRepository.GetAll().ToListAsync(cancellationToken);
    }
}