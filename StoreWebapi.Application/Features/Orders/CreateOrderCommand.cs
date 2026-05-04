
using MediatR;
using StoreWebapi.Application.Common;

public record CreateOrderCommand(
    List<Guid> BookIds,   
    string? CouponCode  
) : IRequest<Result<CreateOrderResponse>>;