using System.Net.Http.Json;
using StoreWebapi.Domain.Common;
using StoreWebapi.Domain.Interfaces;

namespace StoreWebapi.Infrastructure.Shared;

public class GeoLocationService(HttpClient httpClient):IGeoLocationService
{
    public Result<string> EncodeToCellId(double latitude, double longitude, int precision)
    {
        if (latitude < -90 || latitude > 90) 
            return Result.Failure<string>("Invalid Latitude.");

        long x = (long)Math.Floor((longitude + 180) * Math.Pow(2, precision));
        long y = (long)Math.Floor((latitude + 90) * Math.Pow(2, precision));

        return Result.Success($"{precision}:{x}:{y}");
    } 

    public Result<IEnumerable<string>> GetNeighborCellIds(string cellId)
    {
        try 
        {
            var parts = cellId.Split(':');
            int p = int.Parse(parts[0]);
            long x = long.Parse(parts[1]);
            long y = long.Parse(parts[2]);

            var neighbors = new List<string>();
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;
                    neighbors.Add($"{p}:{x + i}:{y + j}");
                }
            }
            return Result.Success<IEnumerable<string>>(neighbors);
        }
        catch
        {
            return Result.Failure<IEnumerable<string>>("Invalid Cell ID format.");
        }
    }

    public async Task<Result<(double lat, double lon)>?> GetLocationFromIpAsync(string ipAddress)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(ipAddress) || ipAddress == "::1")
                ipAddress = "8.8.8.8";
            var response =await httpClient.GetFromJsonAsync<IpApiResponse>($"http://ip-api.com/json/{ipAddress}");
            if (response != null && response.status == "success")
            {
                return Result.Success((response.lat, response.lon));
            }
            return Result.Failure<(double, double)>("External API failed to locate IP.");
        }
        catch (Exception ex)
        {
            
            return Result.Failure<(double, double)>($"Geolocation service unavailable: {ex.Message}");
        }
    }

    private record IpApiResponse(
        string status,
        string country,
        string city,
        double lat,
        double lon,
        string query);
}