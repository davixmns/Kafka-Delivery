using KafkaDelivery.App.Commands;
using KafkaDelivery.Domain.Entities;
using KafkaDelivery.Infra.Repositories;
using MediatR;

namespace KafkaDelivery.App.Handlers;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Customer>
{
    private readonly IRepository<Customer> _customerRepository;
    
    public CreateCustomerCommandHandler(IRepository<Customer> customerRepository)
    {
        _customerRepository = customerRepository;
    }
    
    public async Task<Customer> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var newCustomer = new Customer(request.Name, request.Email, request.PhoneNumber);
        var createdCustomer = await _customerRepository.SaveAsync(newCustomer);
        return createdCustomer;
    }
}