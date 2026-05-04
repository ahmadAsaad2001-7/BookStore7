namespace StoreWebapi.Application.Features.User.GetMyLibrary;

public record GetMyLibraryResponse(Guid BookId, string Title, DateTime PurchaseDate);