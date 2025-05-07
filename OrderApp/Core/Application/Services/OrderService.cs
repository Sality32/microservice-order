using OrderApp.Core.Application.DTOs;
using OrderApp.Core.Application.Interfaces;
using OrderApp.Core.Domain.Entities;
using OrderApp.Core.Domain.Interfaces;

namespace OrderApp.Core.Application.Services;

public class OrderService(
    IOrderRepository orderRepository,
    IUserService userService,
    IProductService productService) : IOrderService
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IUserService _userService = userService;
    private readonly IProductService _productService = productService;

  public async Task<OrderDto> GetByIdAsync(Guid id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null)
            throw new KeyNotFoundException("Order not found");

        var user = await _userService.GetUserAsync(order.UserId);
        return await MapToDto(order, user?.Name ?? "Unknown User");
    }

    public async Task<IEnumerable<OrderDto>> GetAllAsync()
    {
        var orders = await _orderRepository.GetAllAsync();
        var orderDtos = new List<OrderDto>();

        foreach (var order in orders)
        {
            var user = await _userService.GetUserAsync(order.UserId);
            orderDtos.Add(await MapToDto(order, user?.Name ?? "Unknown User"));
        }

        return orderDtos;
    }

    public async Task<IEnumerable<OrderDto>> GetByUserIdAsync(Guid userId)
    {
        var orders = await _orderRepository.GetByUserIdAsync(userId);
        var user = await _userService.GetUserAsync(userId);
        var userName = user?.Name ?? "Unknown User";

        return await Task.WhenAll(
            orders.Select(async order => await MapToDto(order, userName))
        );
    }

    public async Task<OrderDto> CreateAsync(CreateOrderDto createOrderDto)
    {
        // Validate user exists
        var user = await _userService.GetUserAsync(createOrderDto.UserId);
        if (user == null)
            throw new InvalidOperationException("User not found");

        // Validate and get products
        var orderItems = new List<OrderItem>();
        decimal totalAmount = 0;

        foreach (var item in createOrderDto.Items)
        {
            var product = await _productService.GetProductAsync(item.ProductId);
            if (product == null)
                throw new InvalidOperationException($"Product {item.ProductId} not found");

            if (!await _productService.CheckStockAvailabilityAsync(item.ProductId, item.Quantity))
                throw new InvalidOperationException($"Insufficient stock for product {product.Name}");

            var orderItem = new OrderItem
            {
                Id = Guid.NewGuid(),
                ProductId = item.ProductId,
                ProductName = product.Name,
                UnitPrice = product.Price,
                Quantity = item.Quantity,
                Subtotal = product.Price * item.Quantity
            };

            orderItems.Add(orderItem);
            totalAmount += orderItem.Subtotal;
        }

        var order = new Order
        {
            Id = Guid.NewGuid(),
            UserId = createOrderDto.UserId,
            TotalAmount = totalAmount,
            Status = "Created",
            CreatedAt = DateTime.UtcNow,
            Items = orderItems
        };

        await _orderRepository.CreateAsync(order);
        return await MapToDto(order, user.Name);
    }

    public async Task<OrderDto> UpdateStatusAsync(Guid id, UpdateOrderStatusDto updateOrderDto)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null)
            throw new KeyNotFoundException("Order not found");

        order.Status = updateOrderDto.Status;
        order.UpdatedAt = DateTime.UtcNow;

        await _orderRepository.UpdateAsync(order);
        
        var user = await _userService.GetUserAsync(order.UserId);
        return await MapToDto(order, user?.Name ?? "Unknown User");
    }

    private static async Task<OrderDto> MapToDto(Order order, string userName)
    {
        return new OrderDto(
            order.Id,
            order.UserId,
            userName,
            order.TotalAmount,
            order.Status,
            order.CreatedAt,
            order.UpdatedAt,
            order.Items.Select(item => new OrderItemDto(
                item.Id,
                item.ProductId,
                item.ProductName,
                item.UnitPrice,
                item.Quantity,
                item.Subtotal
            ))
        );
    }
}