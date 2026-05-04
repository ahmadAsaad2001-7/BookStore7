using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreWebapi.Application.Features.Admin.CastVote;
using StoreWebapi.Application.Features.Admin.GenerateCoupons;
using StoreWebapi.Application.Features.Admin.GetAllUsers;
using StoreWebapi.Application.Features.Admin.GetVotes;
using StoreWebapi.Application.Features.Books.Queries.GetBooks;

namespace StoreWebapi.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
// [Authorize(Roles = "ADMINISTRATOR")]
public class AdminController :ControllerBase
{
    private readonly ISender  _mediator;

    public AdminController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("getvotes")]
    public async Task<IActionResult> GetVotes()
    {
        var adminIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        var Cmd= new GetVotesQuery(Guid.Parse(adminIdClaim.Value));
        var result = await _mediator.Send(Cmd);
        if (result.IsFailure) return BadRequest(result.Error);
        return Ok(result.Value);
    }
   
    [HttpPost("votes/{voteId:guid}/cast")]
    public async Task<IActionResult> CastVote([FromRoute] Guid voteId, [FromBody] bool IsApproved)
    {
      
        var adminIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (adminIdClaim == null) return Unauthorized();

        var adminId = Guid.Parse(adminIdClaim.Value);

        var command = new CastVoteCommand(voteId, adminId, IsApproved);
        
        var result = await _mediator.Send(command);

        if (result.IsFailure) return BadRequest(result.Error);

        return Ok(result.Value);
    }
    
    [HttpPost("coupons/generate")]
    public async Task<IActionResult> GenerateCoupon( [FromBody] GenerateCouponCommand request)
    {
        var result =await _mediator.Send(request);
        if (result.IsFailure) return BadRequest(result.Error);
        return Ok(result.Value);
    }

    [HttpGet("coupons/")]
    public async Task<IActionResult> GetCoupons()
    { 
      var query = new GetBooksQuery();
      var result = await _mediator.Send(query);
      return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);

    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        var query = new GetAllUsers.GetAllUsersQuery();
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result.Value):  NotFound(result.Error);
    }
    [HttpPost()]
    public async Task<IActionResult> SusbentionVote()
    {
        throw new NotImplementedException();
    }
    
}