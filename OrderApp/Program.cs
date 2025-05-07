using Microsoft.EntityFrameworkCore;
using OrderApp.Core.Application.Interfaces;
using OrderApp.Core.Application.Services;
using OrderApp.Core.Domain.Interfaces;
using OrderApp.Infrastructure.Data.Context;
using OrderApp.Infrastructure.ExternalServices;
using OrderApp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure HTTP clients
builder.Services.AddHttpClient<IUserService, HttpUserService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:UserService"] ?? "http://localhost:7001/");
});

builder.Services.AddHttpClient<IProductService, HttpProductService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ProductService"] ?? "http://localhost:7002/");
});

// Add Services
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
