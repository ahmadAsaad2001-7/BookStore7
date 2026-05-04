using MediatR;
using Microsoft.AspNetCore.Http;
using StoreWebapi.Domain.Domain;
using StoreWebapi.Domain.Interfaces;

namespace StoreWebapi.Application.Features.Vendor.AddVendorLocation.cs;

public class AddVendorLocationHandler : IRequestHandler<AddVendorLocationCommand,Result<AddVendorLocationResponse>>
{
    private readonly IGeoLocationService _geoLocationService;
    private readonly IRepository _repo;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public AddVendorLocationHandler(IGeoLocationService geoLocationService,IRepository repo, 
    IHttpContextAccessor httpContextAccessor)
    {
        _repo = repo;
        _httpContextAccessor = httpContextAccessor;
        _geoLocationService = geoLocationService;
    }
    public async Task<Result<AddVendorLocationResponse>> Handle(AddVendorLocationCommand request, CancellationToken cancellationToken)
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return Result.Failure<AddVendorLocationResponse>("Unauthorized.");
        var userId = Guid.Parse(userIdClaim.Value);
        
        
        var vendor = await _repo.FindById<user>(userId); 
        if (vendor == null) return Result.Failure<AddVendorLocationResponse>("Vendor not found.");
        
        //extract
        var locResult = await _geoLocationService.GetLocationFromIpAsync(request.VendorIpAddress);
        if (locResult == null || locResult.IsFailure)
            return Result.Failure<AddVendorLocationResponse>("Could not determine location from IP.");
        
        var cellResult = _geoLocationService.EncodeToCellId(locResult.Value.lat, locResult.Value.lon, 13);
        if (cellResult.IsFailure) return Result.Failure<AddVendorLocationResponse>(cellResult.Error);
        
        vendor.cellId = cellResult.Value;
     
         
        
        _repo.Update(vendor); 
        await _repo.SaveChangesAsync(cancellationToken);
        var response = new AddVendorLocationResponse
        {
            Id = vendor.Id,
            CellId = vendor.cellId
        };
        return Result.Success(response);
    }
}