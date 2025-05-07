using Microsoft.EntityFrameworkCore;
using UserApp.Core.Application.DTOs;
using UserApp.Core.Application.Interfaces;
using UserApp.Core.Domain.Entities;
using UserApp.Core.Domain.Interfaces;

namespace UserApp.Core.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user != null ? MapToDto(user) : throw new KeyNotFoundException("User not found");
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(MapToDto);
    }

    public async Task<UserDto> CreateAsync(CreateUserDto createUserDto)
    {
        if (await _userRepository.ExistsByEmailAsync(createUserDto.Email))
            throw new InvalidOperationException("Email already exists");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = createUserDto.Name,
            Email = createUserDto.Email,
            CreatedAt = DateTime.UtcNow
        };

        if (!string.IsNullOrEmpty(createUserDto.FirstName) && !string.IsNullOrEmpty(createUserDto.LastName))
        {
            user.Profile = new UserProfile
            {
                Id = Guid.NewGuid(),
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                PhoneNumber = createUserDto.PhoneNumber,
                Address = createUserDto.Address,
                CreatedAt = DateTime.UtcNow
            };
        }

        await _userRepository.CreateAsync(user);
        return MapToDto(user);
    }

    public async Task<UserDto> UpdateAsync(Guid id, UpdateUserDto updateUserDto)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new KeyNotFoundException("User not found");

        if (updateUserDto.Email != null && user.Email != updateUserDto.Email)
        {
            if (await _userRepository.ExistsByEmailAsync(updateUserDto.Email))
                throw new InvalidOperationException("Email already exists");
        }

        user.Name = updateUserDto.Name ?? user.Name;
        user.Email = updateUserDto.Email ?? user.Email;
        user.UpdatedAt = DateTime.UtcNow;

        if (user.Profile == null && !string.IsNullOrEmpty(updateUserDto.FirstName) && !string.IsNullOrEmpty(updateUserDto.LastName))
        {
            user.Profile = new UserProfile
            {
                Id = Guid.NewGuid(),
                FirstName = updateUserDto.FirstName,
                LastName = updateUserDto.LastName,
                PhoneNumber = updateUserDto.PhoneNumber,
                Address = updateUserDto.Address,
                CreatedAt = DateTime.UtcNow
            };
        }
        else if (user.Profile != null)
        {
            user.Profile.FirstName = updateUserDto.FirstName ?? user.Profile.FirstName;
            user.Profile.LastName = updateUserDto.LastName ?? user.Profile.LastName;
            user.Profile.PhoneNumber = updateUserDto.PhoneNumber ?? user.Profile.PhoneNumber;
            user.Profile.Address = updateUserDto.Address ?? user.Profile.Address;
            user.Profile.UpdatedAt = DateTime.UtcNow;
        }

        await _userRepository.UpdateAsync(user);
        return MapToDto(user);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _userRepository.DeleteAsync(id);
    }

    private static UserDto MapToDto(User user)
    {
        return new UserDto(
            user.Id,
            user.Name,
            user.Email,
            user.Profile != null ? new UserProfileDto(
                user.Profile.Id,
                user.Profile.FirstName,
                user.Profile.LastName,
                user.Profile.PhoneNumber,
                user.Profile.Address
            ) : null,
            user.CreatedAt,
            user.UpdatedAt
        );
    }
}