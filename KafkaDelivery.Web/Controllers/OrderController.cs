using KafkaDelivery.App.Commands;
using KafkaDelivery.App.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController: ControllerBase
{
    private readonly IMediator _mediator;
    
    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderRequestDto orderRequestDto)
    {
        var createOrderCommand = new CreateOrderCommand(orderRequestDto);
        
        var result = await _mediator.Send(createOrderCommand);
        
        return result.IsSuccess
            ? Ok(result)
            : BadRequest(result);
    }
}