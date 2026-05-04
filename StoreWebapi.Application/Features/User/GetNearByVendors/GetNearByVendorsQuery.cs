using MediatR;

namespace StoreWebapi.Application.Features.User.GetNearByVendors;

public record GetNearByVendorsQuery(string UserIpAddress) : IRequest<Result<List<GetNearByVendorsResponse>>>;