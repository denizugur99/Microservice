using Microservice.Bus;
using Microservice.Order.Api;
using Microservice.Order.Api.Endpoints.Orders;
using Microservice.Order.Application;
using Microservice.Order.Application.Contracts.Refit.PaymentService;
using Microservice.Order.Application.Contracts.Repositories;
using Microservice.Order.Application.Contracts.UnitOfWorks;
using Microservice.Order.Persistence;
using Microservice.Order.Persistence.Repositories;
using Microservice.Order.Persistence.UnitOfWork;
using Microservices.Shared.Extensions;
using Microservices.Shared.Options;
using Microsoft.EntityFrameworkCore;
using Refit;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCommonServiceExt(typeof(OrderApplicationAssembly));
builder.Services.AddMassTransitExt(builder.Configuration);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});
builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
builder.Services.AddVersionExt();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthenticationExt(builder.Configuration);
builder.Services.AddRefitClient<IPaymentService>().ConfigureHttpClient(cfg =>
{
    var addressUrlOption=builder.Configuration.GetSection(nameof(AddressUrlOption)).Get<AddressUrlOption>();
    cfg.BaseAddress = new Uri(addressUrlOption!.PaymentUrl);
});




var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.AddOrderGroupEndpoints(app.AddVersionSetExt());

app.Run();

