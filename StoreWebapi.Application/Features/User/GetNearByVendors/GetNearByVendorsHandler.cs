using MediatR;
using StoreWebapi.Domain.Domain;
using StoreWebapi.Domain.Interfaces;

namespace StoreWebapi.Application.Features.User.GetNearByVendors;

public class GetNearByVendorsHandler(IGeoLocationService geoService, IRepository repo) 
    : IRequestHandler<GetNearByVendorsQuery, Result<List<GetNearByVendorsResponse>>>
{
    public async Task<Result<List<GetNearByVendorsResponse>>> Handle(GetNearByVendorsQuery request, CancellationToken cancellationToken)
    {
     
        var locResult = await geoService.GetLocationFromIpAsync(request.UserIpAddress);
        

        if (locResult == null || locResult.IsFailure)
            return Result.Failure<List<GetNearByVendorsResponse>>("Could not determine your location.");
        

        var userCellResult = geoService.EncodeToCellId(locResult.Value.lat, locResult.Value.lon, 13);
        if (userCellResult.IsFailure) 
            return Result.Failure<List<GetNearByVendorsResponse>>(userCellResult.Error);

        string userCell = userCellResult.Value;


        var searchArea = new List<string> { userCell };
        var neighborsResult = geoService.GetNeighborCellIds(userCell);
        
        if (neighborsResult.IsSuccess)
        {
            searchArea.AddRange(neighborsResult.Value);
        }


        var nearbyVendors = await repo.FindAll<user>(u => 
           u.cellId != null && searchArea.Contains(u.cellId)
        );


        var response = nearbyVendors.Select(v => new GetNearByVendorsResponse
        {
           VendorId = v.Id,
           VendorName = v.UserName ?? "Unknown Vendor" 
        }).ToList();

  
        return Result.Success(response);
    }
}