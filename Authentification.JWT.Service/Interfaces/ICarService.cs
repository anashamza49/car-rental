using Authentification.JWT.Service.DTOs;

namespace Authentification.JWT.Service.Interfaces
{
    public interface ICarService
    {
        Task<CarResponseDto> AddCarAsync(int ownerId, CarDto carDto);
        Task<List<CarResponseDto>> GetUserCarsAsync();
        Task<bool> DeleteCarAsync(int carId, int ownerId);
        Task<bool> UpdateCarAsync(int ownerId, int carId, CarDto carDto);
        Task<CarResponseDto> GetCarByIdAsync(int carId, int ownerId);
    }
}
