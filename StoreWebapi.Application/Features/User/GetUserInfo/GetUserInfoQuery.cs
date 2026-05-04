using MediatR;

namespace StoreWebapi.Application.Features.User.GetUserInfo;

public record GetUserInfoQuery(Guid Id):IRequest<Result<GetUserInfoResponse>>;