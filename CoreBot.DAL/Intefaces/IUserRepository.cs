﻿// IUserRepository.cs
using CoreBot.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IUserRepository
{
    Task<User> GetUserByIdAsync(string userId);
    Task<List<User>> GetAllUsersAsync();
    Task AddUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(string userId);
    Task AddUserIfNotExistAsync(User user);
}
