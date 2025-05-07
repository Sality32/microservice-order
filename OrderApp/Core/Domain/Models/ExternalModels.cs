namespace OrderApp.Core.Domain.Models;

public record UserDto(Guid Id, string Name, string Email);
public record ProductDto(Guid Id, string Name, string Description, decimal Price, int Stock);