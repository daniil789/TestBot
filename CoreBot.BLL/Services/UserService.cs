// UserService.cs
using CoreBot.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> GetUserByIdAsync(string userId)
    {
        return await _userRepository.GetUserByIdAsync(userId);
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllUsersAsync();
    }

    public async Task AddUserAsync(User user)
    {
        await _userRepository.AddUserAsync(user);
    }

    public async Task UpdateUserAsync(User user)
    {
        await _userRepository.UpdateUserAsync(user);
    }

    public async Task DeleteUserAsync(string userId)
    {
        await _userRepository.DeleteUserAsync(userId);
    }

    public async Task AddUserIfNotExistAsync(string userId, string name, string channel )
    {
        var user = new User
        {
            Id = userId,
            Name = name,
            Channel = channel,
            IsAdmin = false
        };

        await _userRepository.AddUserIfNotExistAsync(user);
    }
}
