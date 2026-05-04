namespace StoreWebapi.Domain.Interfaces;

public interface IGeoLocationService
{
    Task<Result<(double lat, double lon)>> GetLocationFromIpAsync(string ipAddress);
    Result<string> EncodeToCellId(double latitude, double longitude, int precision);
    Result<IEnumerable<string>> GetNeighborCellIds(string cellId);

}