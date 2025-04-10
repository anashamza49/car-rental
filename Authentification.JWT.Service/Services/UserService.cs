﻿using Authentification.JWT.DAL.Models;
using Authentification.JWT.DAL.Repositories;
using Authentification.JWT.Service.DTOs;
using Authentification.JWT.Service.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Authentification.JWT.Service.Services
{
    public class UserService(IUserRepository userRepository, IMapper mapper, ILogger<UserService> logger) : IUserService
    {
        public async Task<UserDto> GetUserByUsernameAsync(string username)
        {
            logger.LogDebug("Looking for the {Username}", username);

            var user = await userRepository.GetUserByUsernameAsync(username);

            if (user == null)
            {
                logger.LogWarning("User {Username} couldn't be found", username);
                return null;
            }

            logger.LogInformation("User {UserId} found", user.Id);
            return mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> RegisterUserAsync(string username, string email, string password, string role = "Employee")
        {
            // Validation du rôle
            if (role != "Admin" && role != "Employee")
                throw new ArgumentException("Invalid role");

            var newUser = new User
            {
                Username = username,
                Email = email,
                PasswordHash = PasswordHasher.HashPassword(password),
                Role = role
            };

            var createdUser = await userRepository.RegisterUserAsync(newUser);
            logger.LogInformation("new user registered : {UserId}", createdUser.Id);

            return mapper.Map<UserDto>(createdUser);
        }

        public async Task<bool> VerifyPassword(int userId, string password)
        {
            logger.LogDebug("verifying password for {UserId}", userId);

            var user = await userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                logger.LogWarning("the user : {UserId} couldn't be found", userId);
                return false;
            }

            var isValid = PasswordHasher.VerifyPassword(password, user.PasswordHash);
            logger.LogInformation("verifying password {UserId} : {Result}", userId, isValid ? "Valid" : "Invalid");

            return isValid;
        }
    }
}