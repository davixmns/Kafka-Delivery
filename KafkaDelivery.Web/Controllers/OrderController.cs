using KafkaDelivery.App.Services;
using KafkaDelivery.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController: ControllerBase
{
    private readonly IOrderService _orderService;
    
    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] Order order)
    {
        var createOrderResult = await _orderService.CreateOrderAsync(order);
        
        return createOrderResult.IsSuccess
            ? Ok(createOrderResult)
            : BadRequest(createOrderResult);
    }
}