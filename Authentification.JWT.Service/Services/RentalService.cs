using Authentication.Shared.DTOs;
using Authentification.JWT.DAL.Models;
using Authentification.JWT.DAL.Repositories;
using Authentification.JWT.Service.Interfaces;
using AutoMapper;

namespace Authentification.JWT.Service.Services
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository rentalRepo;
        private readonly ICarRepository carRepo;
        private readonly IUserRepository userRepo;

        public RentalService(IRentalRepository rentalRepo, ICarRepository carRepo, IUserRepository userRepo)
        {
            this.rentalRepo = rentalRepo;
            this.carRepo = carRepo;
            this.userRepo = userRepo;
        }

        public async Task<RentalResponseDto> RentCarAsync(int userId, RentalDto rentDto)
        {
            if (await rentalRepo.IsCarRented(rentDto.CarId, rentDto.StartDate, rentDto.EndDate))
                throw new Exception("Car already rented in this period");

            var rental = new Rental
            {
                CarId = rentDto.CarId,
                UserId = userId,
                StartDate = rentDto.StartDate,
                EndDate = rentDto.EndDate
            };

            await rentalRepo.AddAsync(rental);

            var car = await carRepo.GetByIdAsync(rentDto.CarId);
            var user = await userRepo.GetUserByIdAsync(userId);

            return new RentalResponseDto
            {
                Id = rental.Id,
                CarModel = car.Model,
                UserEmail = user.Email,
                StartDate = rental.StartDate,
                EndDate = rental.EndDate
            };
        }

        public async Task<List<RentalResponseDto>> GetAllRentalsAsync()
        {
            var rentals = await rentalRepo.GetAllAsync();
            var result = new List<RentalResponseDto>();

            foreach (var rental in rentals)
            {
                var car = await carRepo.GetByIdAsync(rental.CarId);
                var user = await userRepo.GetUserByIdAsync(rental.UserId);

                result.Add(new RentalResponseDto
                {
                    Id = rental.Id,
                    CarModel = car.Model,
                    UserEmail = user.Email,
                    StartDate = rental.StartDate,
                    EndDate = rental.EndDate
                });
            }

            return result;
        }
    }
}
