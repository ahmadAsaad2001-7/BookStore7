using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StoreWebapi.Domain.Interfaces;
using StoreWebapi.Infrastructure.Data;
using StoreWebapi.Infrastructure.Shared;


namespace StoreWebapi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfraStructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
        services.AddScoped<IPaymentService, StripePaymentService>();
        services.AddScoped<IGeoLocationService, GeoLocationService>();
        services.AddScoped<IRepository, Repository>();
        return services;
        

    }
}