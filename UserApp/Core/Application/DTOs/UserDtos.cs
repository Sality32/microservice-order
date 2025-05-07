namespace UserApp.Core.Application.DTOs;

public record UserDto(
    Guid Id, 
    string Name, 
    string Email, 
    UserProfileDto? Profile,
    DateTime CreatedAt, 
    DateTime? UpdatedAt
);

public record CreateUserDto(
    string Name, 
    string Email,
    string? FirstName,
    string? LastName,
    string? PhoneNumber,
    string? Address
);

public record UpdateUserDto(
    string? Name,
    string? Email,
    string? FirstName,
    string? LastName,
    string? PhoneNumber,
    string? Address
);

public record UserProfileDto(
    Guid Id,
    string FirstName,
    string LastName,
    string? PhoneNumber,
    string? Address
);