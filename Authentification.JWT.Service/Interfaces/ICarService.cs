using Authentification.JWT.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentification.JWT.Service.Interfaces
{
    public interface ICarService
    {
        Task<CarResponseDto> AddCarAsync(int ownerId, CarDto carDto);
        Task<List<CarResponseDto>> GetUserCarsAsync(int ownerId);
        Task<bool> DeleteCarAsync(int carId, int ownerId);
    }
}
