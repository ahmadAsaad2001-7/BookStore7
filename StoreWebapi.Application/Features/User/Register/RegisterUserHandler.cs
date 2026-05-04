using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using StoreWebapi.Application.Common;
using StoreWebapi.Domain.Domain;
using StoreWebapi.Domain.Domain.Enums;
using StoreWebapi.Domain.Interfaces;

namespace StoreWebapi.Application.Features.User;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Result<Guid>>
{
    private readonly UserManager<user> _userManager;
    private readonly IGeoLocationService _geoService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRepository _repo; 

    public RegisterUserHandler(
        UserManager<user> userManager,
        IGeoLocationService geoService,
        IHttpContextAccessor httpContextAccessor,
        IRepository repo) 
    {
        _userManager = userManager;
        _geoService = geoService;
        _httpContextAccessor = httpContextAccessor;
        _repo = repo;
    }

    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {

        var existing = await _userManager.FindByEmailAsync(request.Email);
        if (existing != null)
            return Result.Failure<Guid>("User already exists with this email.");

     
        var userIpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
        if (string.IsNullOrEmpty(userIpAddress) || userIpAddress == "::1" || userIpAddress == "127.0.0.1")
        {
            userIpAddress = "8.8.8.8"; 
        }

        string? calculatedCellId = null;
        var locResult = await _geoService.GetLocationFromIpAsync(userIpAddress);

   
        if (locResult?.IsSuccess == true)
        {
            var precision = 13;
            var cellResult = _geoService.EncodeToCellId(locResult.Value.lat, locResult.Value.lon, precision);
            
            if (cellResult.IsSuccess)
            {
                calculatedCellId = cellResult.Value;

             
                var existingCell = await _repo.FindById<gridCell>(calculatedCellId);
                
                if (existingCell == null)
                {
                    var newGridCell = new gridCell
                    {
                        cellId = calculatedCellId,
                        latitude = locResult.Value.lat,
                        longitude = locResult.Value.lon,
                        precision = precision
                    };
                    
                    _repo.Add(newGridCell);
      
                    await _repo.SaveChangesAsync(cancellationToken);
                }
            }
        }

        var newUser = new user
        {
            Id = Guid.NewGuid(),
            UserName = string.IsNullOrWhiteSpace(request.UserName) ? request.Email : request.UserName,
            Email = request.Email,
            imageUrl = request.ImageUrl ?? string.Empty,
            isSusbended = false,
            Transactions = new List<transaction>(),
            VendorTransactions = new List<transaction>(),
            Comments = new List<comment>(),
            CouponUsers = new List<couponUser>(),
            VotesInitiated = new List<vote>(),
            ParticipatingVotes = new List<voteParticipant>(),
            cellId = calculatedCellId 
        };

        var createResult = await _userManager.CreateAsync(newUser, request.Password);
    
        if (!createResult.Succeeded)
        {
            var errors = string.Join("; ", createResult.Errors.Select(e => e.Description));
            return Result.Failure<Guid>($"User creation failed: {errors}");
        }

        var roleResult = await _userManager.AddToRoleAsync(newUser, Roles.USER.ToString());
    
        if (!roleResult.Succeeded)
        {
            await _userManager.DeleteAsync(newUser); 
            var roleErrors = string.Join("; ", roleResult.Errors.Select(e => e.Description));
            return Result.Failure<Guid>($"Failed to add role: {roleErrors}");
        }

        return Result.Success(newUser.Id);
    }
}