using MediatR;
using Microsoft.AspNetCore.Identity;
using StoreWebapi.Domain.Domain;
using StoreWebapi.Domain.Interfaces;

namespace StoreWebapi.Application.Features.User.GetUserInfo;

public class GetUserInfoHandler(IRepository repo, UserManager<user> userManager)
    : IRequestHandler<GetUserInfoQuery, Result<GetUserInfoResponse>>
{
    public async Task<Result<GetUserInfoResponse>> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        var u = await repo.FindById<user>(request.Id);
        if (u is null)
        {
            return Result.Failure<GetUserInfoResponse>("User not found.");
        }

        // جلب الأدوار الحقيقية كنصوص من نظام Identity
        var userRoles = await userManager.GetRolesAsync(u);

        var response = new GetUserInfoResponse
        {
            Id = u.Id,
            imageUrl = u.imageUrl,
            email = u.Email,
            UserName = u.UserName,
            isSusbended = u.isSusbended,
            cellId = u.cellId,
            // إرسال قائمة النصوص مباشرة
            roles = userRoles.ToList()
        };

        return Result.Success(response);
    }
}