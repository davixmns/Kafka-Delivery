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
        var createOrderCommand = new CreateOrderCommand(orderRequestDto.Products, orderRequestDto.CustomerId);
        
        var result = await _mediator.Send(createOrderCommand);
        
        return result.IsSuccess
            ? Ok(result)
            : BadRequest(result);
    }
    
    [HttpPut]
    [Route("PayOrder")]
    public async Task<IActionResult> PayOrder(PayOrderRequestDto dto)
    {
        var payOrderCommand = new PayOrderCommand(dto.OrderId, dto.PaymentMethod);
        
        var result = await _mediator.Send(payOrderCommand);
        
        return result.IsSuccess
            ? Ok(result)
            : BadRequest(result);
    }
    
    [HttpPut]
    [Route("CancelOrder/{orderId}")]
    public async Task<IActionResult> CancelOrder(int orderId)
    {
        var cancelOrderCommand = new CancelOrderCommand(orderId);
        
        var result = await _mediator.Send(cancelOrderCommand);
        
        return result.IsSuccess
            ? Ok(result)
            : BadRequest(result);
    }
    
}