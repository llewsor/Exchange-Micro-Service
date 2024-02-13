using ExchangeService.Entity;
using ExchangeService.Repositories;
using ExchangeService.Repositories.Interfaces;
using ExchangeService.Services;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var appConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ExchangeDbContext>(options => options.UseSqlServer(appConnectionString));

builder.Services.AddMemoryCache();

builder.Services.AddHttpClient("ExchangeRatesAPI", httpClient =>
{
    var configuration = builder.Configuration.GetSection("ExchangeIntegrationAPI");

    var baseURL = configuration["baseURL"];
    var key = configuration["key"];

    httpClient.BaseAddress = new Uri($"{baseURL}latest?access_key={key}");
});

//Register Scope
builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
builder.Services.AddScoped<IRateProviderService, RateProviderService>();

// Configure Serilog
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.File("logs/ExchangeService.log", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Information));

var app = builder.Build(); 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor |
    ForwardedHeaders.XForwardedProto
});

app.UseAuthorization();

app.MapControllers();

app.Run();
