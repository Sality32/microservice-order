using OrderApp.Core.Application.DTOs;

namespace OrderApp.Core.Application.Interfaces;

public interface IOrderService
{
    Task<OrderDto> GetByIdAsync(Guid id);
    Task<IEnumerable<OrderDto>> GetAllAsync();
    Task<IEnumerable<OrderDto>> GetByUserIdAsync(Guid userId);
    Task<OrderDto> CreateAsync(CreateOrderDto createOrderDto);
    Task<OrderDto> UpdateStatusAsync(Guid id, UpdateOrderStatusDto updateOrderDto);
}