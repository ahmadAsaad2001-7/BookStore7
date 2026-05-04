using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreWebapi.Application.Features.Orders;
using StoreWebapi.Application.Features.Orders.ConfirmOrderPayment;

namespace StoreWebapi.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class OrdersController(ISender mediator) : ControllerBase
{
    
    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout([FromBody] CreateOrderCommand command)
    {
        var result = await mediator.Send(command);
        
        return result.IsSuccess 
            ? Ok(result.Value) 
            : BadRequest(result.Error);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ConfirmOrder([FromRoute] Guid id)
    {
        var q = new ConfirmOrderPaymentCommand(id);
        var result =await mediator.Send(q);
        return result.IsSuccess? Ok() :NotFound(result.Error);
    }
    
}