using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreWebapi.Application.Features.Vendor.AddVendorLocation.cs;
using StoreWebapi.Application.Features.Vendor.Sell;

namespace StoreWebapi.Api.Controllers;
[Authorize(Roles = "VENDOR")]
[ApiController]
[Route("api/[controller]")]
public class VendorController : ControllerBase
{
    private readonly ISender _mediator;

    public VendorController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("sellbook")]
    public async Task<IActionResult> SellbookController([FromBody] SellBookCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return  BadRequest(result.Error);
        }
        return Ok(result.Value);
        
    }
    [Authorize(Roles = "Vendor")] 
    [ApiController]
    [Route("api/[controller]")]
    public class VendorsController(ISender mediator) : ControllerBase
    {
        [HttpPost("location")]
        public async Task<IActionResult> AddLocation([FromBody] AddVendorLocationCommand request)
        {
           
            var ipAddress = string.IsNullOrWhiteSpace(request.VendorIpAddress) 
                ? HttpContext.Connection.RemoteIpAddress?.ToString() 
                : request.VendorIpAddress;

            if (string.IsNullOrEmpty(ipAddress))
            {
                return BadRequest("Could not determine IP address.");
            }

            var command = new AddVendorLocationCommand { VendorIpAddress = ipAddress };
            var result = await mediator.Send(command);
 
            return result.IsSuccess 
                ? Ok(result.Value) 
                : BadRequest(result.Error);
        }
    }

}