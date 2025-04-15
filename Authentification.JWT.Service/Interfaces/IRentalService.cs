using Authentication.Shared.DTOs;

namespace Authentification.JWT.Service.Interfaces
{
    public interface IRentalService
    {
        Task<RentalResponseDto> RentCarAsync(int userId, RentalDto rentDto);
        Task<List<RentalResponseDto>> GetAllRentalsAsync();
    }

}
