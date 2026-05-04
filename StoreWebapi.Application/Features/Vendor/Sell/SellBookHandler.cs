using System.Data;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using StoreWebapi.Application.Common;
using StoreWebapi.Domain.Domain;
using StoreWebapi.Domain.Domain.Enums;
using StoreWebapi.Domain.Interfaces;

namespace StoreWebapi.Application.Features.Vendor.Sell;

public class SellBookHandler(IRepository repo , IHttpContextAccessor httpContextAccessor):IRequestHandler<SellBookCommand,Result<SellBookResponse>>
{
    public async Task<Result<SellBookResponse>> Handle(SellBookCommand request, CancellationToken cancellationToken)
    {
       var UserIdClaims= httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
       if (UserIdClaims == null)
           return Result.Failure<SellBookResponse>("User is not Authenticated");
       var vendorId=Guid.Parse(UserIdClaims.Value);
       var newBook = new Book
       {
           id = Guid.NewGuid(),
           name=request.Name,
           description = request.Description,
           author =  request.Author,
           price = request.Price,
           isbn =  request.Isbn,
           imageUrl = request.ImageUrl,
           UserId = vendorId,
           genres = request.Genres,
           CreatedAt =  DateTime.UtcNow
           
       };
        repo.Add(newBook);
        await repo.SaveChangesAsync(cancellationToken);
        var response= new SellBookResponse
        {
            Id=newBook.id,
            Name=newBook.name,
            Description=newBook.description,
            CreatedAt =  newBook.CreatedAt,
        };
  
        return Result.Success(response);

    }
}