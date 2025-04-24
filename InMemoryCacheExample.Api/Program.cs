using DapperCacheExample.Data;
using InMemoryCacheExample.Service.Contracts;
using InMemoryCacheExample.Service.Implementations;
using InMemoryCacheExample.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddMemoryCache();

builder.Services.AddSingleton<DapperContext>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.Configure<CacheSettings>(
    builder.Configuration.GetSection("CacheSettings")
);
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddResponseCaching();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseResponseCaching();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
