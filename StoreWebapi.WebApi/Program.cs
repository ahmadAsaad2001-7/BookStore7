using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

using StoreWebapi.Application;
using StoreWebapi.Domain.Domain;
using StoreWebapi.Domain.Interfaces;
using StoreWebapi.Infrastructure;
using StoreWebapi.Infrastructure.Data;
using StoreWebapi.Infrastructure.Shared;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentity<user, IdentityRole<Guid>>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 12;
        
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = CookieAuthenticationDefaults.AuthenticationScheme;
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        
        // FIX: Use conditional security based on environment
        options.Cookie.SecurePolicy = builder.Environment.IsDevelopment() 
            ? CookieSecurePolicy.SameAsRequest 
            : CookieSecurePolicy.Always;
        
        options.Cookie.SameSite = SameSiteMode.None;
        
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/api/auth/logout";
        options.AccessDeniedPath = "/api/auth/access-denied";
    });

builder.Services.AddOpenApi();
builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }); 
    
builder.Services.AddApplication();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

builder.Services.AddInfraStructure(builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp", policy =>
        policy.WithOrigins(
                "http://localhost:5173",
                "http://localhost:3000", 
                "https://bstore-vue-ql9e.vercel.app",
                "https://bstore-vue-ql9e-git-master-ahmedasaad71421-1017s-projects.vercel.app",
                "https://bstore-vue-ql9e-4th59s4hx-ahmedasaad71421-1017s-projects.vercel.app"
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AllowVueApp");
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    await IdentitySeeder.SeedAdminAsync(scope.ServiceProvider);
}

app.Run();