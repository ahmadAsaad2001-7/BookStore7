using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreWebapi.Application.Features.Books.Commands.Create;
using StoreWebapi.Application.Features.Books.Commands.Delete;
using StoreWebapi.Application.Features.Books.Commands.Update;
using StoreWebapi.Application.Features.Books.Queries;
using StoreWebapi.Application.Features.Books.Queries.GetBooks;
using StoreWebapi.Application.Features.Books.SearchBooks;

namespace StoreWebapi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly ISender _mediator;


    public BooksController(ISender mediator)
    {
        _mediator = mediator;
    }
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetBooksQuery();
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);

    }   
    [AllowAnonymous ]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetBook([FromRoute] Guid id)
    {
        var rolees = User.FindAll(System.Security.Claims.ClaimTypes.Role).Select(c => c.Value);
        Console.WriteLine($"Current User Roles: {string.Join(", ", rolees)}");
        Console.WriteLine($"Received id: {id}");
        Console.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return BadRequest(new { errors });
        }


        var query = new GetBookByIdQuery(id);
        Console.WriteLine($"Controller created query with Id: {query.Id}");

        try
        {
            var result = await _mediator.Send(query);


            if (result.IsFailure)
            {

                return result.Error!.Contains("not found")
                    ? NotFound(result.Error)
                    : BadRequest(result.Error);
            }

            return Ok(result.Value);
        }
        catch (Exception ex)
        {var roles = User.FindAll(System.Security.Claims.ClaimTypes.Role).Select(c => c.Value);
            Console.WriteLine($"Current User Roles: {string.Join(", ", roles)}");
            return StatusCode(500, $"Internal error: {ex.Message}");
        }
    }
    [Authorize(Roles ="Administrator") ]
    [HttpPost]
    public async Task<IActionResult> AddBook([FromBody] CreateCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }
  
    [HttpDelete]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteBook([FromQuery] Guid id)
    {
        var Cmd = new DeleteBookCommand(id);
        var result = await _mediator.Send(Cmd);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);

        }

        return Ok(result.Value);
    }
   

    [HttpPut("{id:guid}")] 
    public async Task<IActionResult> UpdateBook([FromRoute] Guid id, [FromBody] UpdateBookCommand command)
    {
       
        command.Id = id;
        var result =await _mediator.Send(command);
        if (result.IsFailure)
        {
            return result.Error!.Contains("not found")
                ? NotFound(new { error = result.Error })
                : BadRequest(new { error = result.Error });
        }
        return Ok(result.Value); 
    }

}