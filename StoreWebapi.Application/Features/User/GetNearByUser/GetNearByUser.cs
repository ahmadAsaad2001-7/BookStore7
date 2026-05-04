using MediatR;
using StoreWebapi.Domain.Domain;
using StoreWebapi.Domain.Interfaces;

namespace StoreWebapi.Application.Features.User.GetNearByUser;


public record GetNearByUsersRequest(string UserIpAddress) : IRequest<Result<List<GetNearByUsersResponse>>>;

public record GetNearByUsersResponse
{
    public Guid UserId { get; set; }
    public string ImageUrl { get; set; }
    public string UserName { get; set; }
    public bool IsSuspended { get; set; } 
    public string? CellId { get; set; }
};

public class GetNearByUsersHandler(IGeoLocationService geoService, IRepository repo) 
    : IRequestHandler<GetNearByUsersRequest, Result<List<GetNearByUsersResponse>>>
{
    public async Task<Result<List<GetNearByUsersResponse>>> Handle(GetNearByUsersRequest request, CancellationToken cancellationToken)
    {

        var locResult = await geoService.GetLocationFromIpAsync(request.UserIpAddress);

        if (locResult == null || locResult.IsFailure)
            return Result.Failure<List<GetNearByUsersResponse>>("Could not determine your location.");


        var userCellResult = geoService.EncodeToCellId(locResult.Value.lat, locResult.Value.lon, 13);
        if (userCellResult.IsFailure) 
            return Result.Failure<List<GetNearByUsersResponse>>(userCellResult.Error);

        string userCell = userCellResult.Value;


        var searchArea = new List<string> { userCell };
        var neighborsResult = geoService.GetNeighborCellIds(userCell);
        
        if (neighborsResult.IsSuccess)
        {
            searchArea.AddRange(neighborsResult.Value);
        }

        var nearbyUsers = await repo.FindAll<user>(u => 
            u.cellId != null && searchArea.Contains(u.cellId)
        );

        var response = nearbyUsers.Select(v => new GetNearByUsersResponse
        {
            UserId = v.Id,
            UserName = v.UserName ?? "Unknown User",
            ImageUrl = v.imageUrl ?? "https://via.placeholder.com/150",
            IsSuspended = v.isSusbended,
            CellId = v.cellId
        }).ToList();

        return Result.Success(response);
    }
}