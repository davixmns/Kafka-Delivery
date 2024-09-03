using KafkaDelivery.Domain.Entities;
using KafkaDelivery.Infra.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;
    
    public CustomerController(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    
    [HttpGet]
    public IActionResult GetAll()
    {
        var customers = _customerRepository.GetAll();
        return Ok(customers);
    }
    
    [HttpPost]
    public IActionResult Create([FromBody] Customer customer)
    {
        _customerRepository.Save(customer);
        return Ok();
    }
}