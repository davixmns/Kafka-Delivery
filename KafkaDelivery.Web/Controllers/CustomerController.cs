using KafkaDelivery.Domain.Entities;
using KafkaDelivery.Infra.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
    private readonly IRepository<Customer> _customerRepository;
    private readonly IRepository<Order> _orderRepository;
    
    public CustomerController(IRepository<Customer> customerRepository, IRepository<Order> orderRepository)
    {
        _customerRepository = customerRepository;
        _orderRepository = orderRepository;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var customers = await _customerRepository.GetAll().ToListAsync();
        
        return Ok(customers);
    }
    
    [HttpPost]
    public IActionResult Create([FromBody] Customer customer)
    {
        _customerRepository.SaveAsync(customer);
        return Ok(customer);
    }
    
    [HttpGet("{customerId}")]
    public IActionResult GetCustomerOrders(string customerId)
    {
        var customer = _orderRepository.GetAll()
            .Where(x => x.Customer.CustomerId == customerId);
        
        return Ok(customer);
    }
}