using OrderApp.Core.Domain.Models;

namespace OrderApp.Core.Application.Interfaces;

public interface IUserService
{
    Task<UserDto?> GetUserAsync(Guid userId);
}

public interface IProductService
{
    Task<ProductDto?> GetProductAsync(Guid productId);
    Task<bool> CheckStockAvailabilityAsync(Guid productId, int quantity);
}