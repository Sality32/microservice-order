namespace OrderApp.Core.Application.DTOs;

public record OrderDto(
    Guid Id,
    Guid UserId,
    string UserName,
    decimal TotalAmount,
    string Status,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    IEnumerable<OrderItemDto> Items
);

public record OrderItemDto(
    Guid Id,
    Guid ProductId,
    string ProductName,
    decimal UnitPrice,
    int Quantity,
    decimal Subtotal
);

public record CreateOrderDto(
    Guid UserId,
    List<CreateOrderItemDto> Items
);

public record CreateOrderItemDto(
    Guid ProductId,
    int Quantity
);

public record UpdateOrderStatusDto(string Status);