using DapperCacheExample.Data;
using InMemoryCacheExample.Data.Repositories.Contracts;
using InMemoryCacheExample.Data.Repositories.Implementations;
using InMemoryCacheExample.Service.Contracts;
using InMemoryCacheExample.Service.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddMemoryCache();

builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

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
