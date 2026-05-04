using MediatR;
using StoreWebapi.Domain.Domain;
using StoreWebapi.Domain.Interfaces;

namespace StoreWebapi.Application.Features.User.GetUserInfo;

public class GetUserInfoHandler(IRepository repo) :IRequestHandler<GetUserInfoQuery,Result<GetUserInfoResponse>>
{
    public async Task<Result<GetUserInfoResponse>> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        var U = await repo.FindById<user>(request.Id);
        if (U is null)
        {
            return Result.Failure<GetUserInfoResponse>("Book with the specified ID was not found.");
        }
        var response = new GetUserInfoResponse
        {
        Id = U.Id,
        imageUrl = U.imageUrl,
        cellId = U.cellId,
        isSusbended = U.isSusbended,
        };
        return Result.Success(response);

    }
}