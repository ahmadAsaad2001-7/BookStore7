using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using StoreWebapi.Application.Features.User;
using StoreWebapi.Application.Features.User.ApplyforVendorRole;
using StoreWebapi.Application.Features.User.GetMyLibrary;
using StoreWebapi.Application.Features.User.GetNearByUser;
using StoreWebapi.Application.Features.User.GetNearByVendors;
using StoreWebapi.Application.Features.User.GetSearchResult;
using StoreWebapi.Application.Features.User.GetUserInfo;

namespace StoreWebapi.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand loginRequest)
    {
        var result = await _mediator.Send(loginRequest);
        if(result.IsSuccess)
            return Ok(result.Value);
        return BadRequest(new { error = result.Error });
    }
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok(new { message = "Logged out" });
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        var result = await _mediator.Send(command);
    
        return result.IsSuccess 
            ? Ok(new { userId = result.Value, message = "Registration successful. Please login." })
            : BadRequest(new { error = result.Error }); // ✅ 400, not 401!
    }
    
    [HttpGet("check-auth")]
    [Authorize] 
    public IActionResult CheckAuth()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userName = User.Identity?.Name;
        var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value);

        return Ok(new 
        {
            authenticated = true,
            user = new 
            {
                id = userId,
                name = userName,
                roles = roles
            }
        });
    }
    

    
    [Authorize(Roles = "USER")] 
    [HttpPost("apply-for-vendor")]
    public async Task<IActionResult> ApplyForVendor()
    {
     
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return Unauthorized("User ID not found in claims.");

        var userId = Guid.Parse(userIdClaim.Value);

       
        var command = new ApplyForVendorCommand(userId);
    
        var result = await _mediator.Send(command);

        return result.IsSuccess 
            ? Ok(new { VoteId = result.Value, Message = "Application submitted to admins." }) 
            : BadRequest(result.Error);
    }

    [HttpGet("GetNearbyVendors")]
    public async Task<IActionResult> GetNearbyVendors()
    {

        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        if (string.IsNullOrEmpty(ipAddress) || ipAddress == "::1")
        {
            ipAddress = "168.63.69.204"; 
        }
        var query= new GetNearByVendorsQuery(ipAddress);
        var result = await _mediator.Send(query);
        return Ok(result);

    }

    [HttpGet("GetNearbyUsers")]
    public async Task<IActionResult> GetNearByUser()
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        if (string.IsNullOrEmpty(ipAddress) || ipAddress == "::1")
        {
            ipAddress = "8.8.8.8"; 
        }
        var query= new GetNearByUsersRequest(ipAddress);
        var result = await _mediator.Send(query);
        return Ok(result);

    }

    [HttpPost("GetSearchResults")] 
    public async Task<IActionResult> GetSearchResults([FromBody] GetSearchResultQuery query)
    {
        var result = await _mediator.Send(query);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
    
    [HttpGet("Library")]
    public async Task<IActionResult> GetLibrary([FromQuery] Guid? userId)
    {
        var query = new GetMyLibraryQuery(userId);
        var result = await _mediator.Send(query);
        return result.IsSuccess?Ok(result):result.Error.Contains("UnAuthorized") 
            ? Unauthorized(result) : Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUser([FromRoute] Guid Id)
    {
        
        var query = new GetUserInfoQuery(Id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    [Authorize]
    [HttpGet("myinfo")]
    public async Task<IActionResult> GetMyInfo()
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                                ?? throw new UnauthorizedAccessException("User ID not found"));
        
        var result = await _mediator.Send(new GetUserInfoQuery(userId));
        return result.IsSuccess ? Ok(result.Value) : NotFound(new { error = "User not found" });
    }
    
    [HttpPost("coupons/validate")]
    public async Task<IActionResult> ValidateCoupon([FromBody] GetCouponByCodeQuery request)
    {
        var query = new GetCouponByCodeQuery(request.code);
        var result = await _mediator.Send(query);

        if (result.IsSuccess && result.Value is not null)
        {
            return Ok(result.Value);
        }
    
        return BadRequest(new { message = result.Error });
    }
    
    
}