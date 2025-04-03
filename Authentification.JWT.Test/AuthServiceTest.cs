using Authentification.JWT.DAL.Models;
using Authentification.JWT.DAL.Repositories;
using Authentification.JWT.Service.DTOs;
using Authentification.JWT.Service.Services;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Authentification.JWT.Test
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockRepo = new();
        private readonly Mock<ILogger<UserService>> _mockLogger = new();
        private readonly IMapper _mapper;

        public UserServiceTests()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDto>());
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task RegisterUserAsync_Returns_UserDto_When_Successful()
        {
            // arrange
            var testUser = new User { Id = 1, Username = "anashamza", Email = "anas@hotmail.com" };
            _mockRepo.Setup(r => r.RegisterUserAsync(It.IsAny<User>()))
                .ReturnsAsync(testUser);

            var service = new UserService(_mockRepo.Object, _mapper, _mockLogger.Object);

            // act
            var result = await service.RegisterUserAsync("anashamza", "anas@hotmail.com", "SqLiEch49");

            // assert
            Assert.Equal("testuser", result.Username);

            // verifying of log doese it match real msg
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("new user registered")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task GetUserByUsernameAsync_Returns_UserDto_When_UserExists()
        {
            // arrange
            var testUser = new User { Id = 1, Username = "anashamza", Email = "anas@hotmail.com" };
            _mockRepo.Setup(r => r.GetUserByUsernameAsync("anashamza"))
                   .ReturnsAsync(testUser);

            var service = new UserService(_mockRepo.Object, _mapper, _mockLogger.Object);

            // act
            var result = await service.GetUserByUsernameAsync("anashamza");

            // assert
            Assert.NotNull(result);
            Assert.Equal("anashamza", result.Username);
        }

        [Fact]
        public async Task VerifyPassword_Returns_True_When_Credentials_Valid()
        {
            // arrange
            const string password = "SqLiEch49";
            var hashedPassword = PasswordHasher.HashPassword(password);
            var testUser = new User { Id = 1, PasswordHash = hashedPassword };

            _mockRepo.Setup(r => r.GetUserByIdAsync(1))
                   .ReturnsAsync(testUser);

            var service = new UserService(_mockRepo.Object, _mapper, _mockLogger.Object);

            // act
            var result = await service.VerifyPassword(1, password);

            // assert
            Assert.True(result);
        }
    }
}