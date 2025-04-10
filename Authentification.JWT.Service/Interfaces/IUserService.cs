﻿using Authentification.JWT.Service.DTOs;

namespace Authentification.JWT.Service.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetUserByUsernameAsync(string username);

        Task<UserDto> RegisterUserAsync(string username, string email, string password,
            string role = "Employee");
        Task<bool> VerifyPassword(int userId, string password);
    }
}
