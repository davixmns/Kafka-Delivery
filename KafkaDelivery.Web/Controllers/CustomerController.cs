using FluentValidation;
using KafkaDelivery.App.Commands;
using KafkaDelivery.App.Dtos;
using KafkaDelivery.App.Validators;
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
        var command = new GetAllCustomersQuery();
        
        var customers = await _mediator.Send(command);
        
        return Ok(customers);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomer(CreateCustomerRequestDto dto)
    {
        var command = new CreateCustomerCommand(dto.Name, dto.Email, dto.PhoneNumber);
        
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