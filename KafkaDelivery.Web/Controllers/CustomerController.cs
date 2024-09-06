using KafkaDelivery.App.Commands;
using KafkaDelivery.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public CustomerController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllCustomers()
    {
        var command = new GetAllCustomersCommand();
        
        var customers = await _mediator.Send(command);
        
        return Ok(customers);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateCustomer(Customer customer)
    {
        var command = new CreateCustomerCommand(customer.Name, customer.Email, customer.PhoneNumber);
        
        var createdCustomer = await _mediator.Send(command);
        
        return Ok(createdCustomer);
    }
    
    [HttpGet("{customerId}")]
    public async Task<IActionResult> GetCustomerOrders(int customerId)
    {
        var command = new GetCustomerOrdersQuery(customerId);
        
        var customerOrders = await _mediator.Send(command);
        
        return Ok(customerOrders);
    }
}