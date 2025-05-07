using System.Net.Http.Json;
using OrderApp.Core.Application.Interfaces;
using OrderApp.Core.Domain.Models;

namespace OrderApp.Infrastructure.ExternalServices;

public class HttpUserService : IUserService
{
    private readonly HttpClient _httpClient;

    public HttpUserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<UserDto?> GetUserAsync(Guid userId)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<UserDto>($"api/users/{userId}");
        }
        catch
        {
            return null;
        }
    }
}

public class HttpProductService : IProductService
{
    private readonly HttpClient _httpClient;

    public HttpProductService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ProductDto?> GetProductAsync(Guid productId)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<ProductDto>($"api/products/{productId}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> CheckStockAvailabilityAsync(Guid productId, int quantity)
    {
        try
        {
            var product = await GetProductAsync(productId);
            return product?.Stock >= quantity;
        }
        catch
        {
            return false;
        }
    }
}