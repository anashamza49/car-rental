using Authentification.JWT.DAL.Models;
using Authentification.JWT.DAL.Repositories;
using Authentification.JWT.Service.DTOs;
using Authentification.JWT.Service.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Authentification.JWT.Service.Services
{
    public class CarService : ICarService
    {
        private readonly ICarRepository carRepo;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment webHostEnvironment;

        public CarService(ICarRepository carRepo, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            this.carRepo = carRepo;
            this.mapper = mapper;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<CarResponseDto> AddCarAsync(int ownerId, CarDto carDto)
        {
            try
            {
                var car = mapper.Map<Car>(carDto);
                car.OwnerId = ownerId;

                if (!string.IsNullOrEmpty(carDto.ImageUrl))
                {
                    byte[] imageBytes = Convert.FromBase64String(carDto.ImageUrl);

                    var fileName = Guid.NewGuid().ToString() + ".png";
                    var filePath = Path.Combine(webHostEnvironment.WebRootPath, "car_images", fileName);
                    var directoryPath = Path.GetDirectoryName(filePath);

                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    await File.WriteAllBytesAsync(filePath, imageBytes);

                    car.ImageUrl = "/car_images/" + fileName;
                }

                var createdCar = await carRepo.AddAsync(car);
                return mapper.Map<CarResponseDto>(createdCar);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error adding car: {ex.Message}");
                throw new ApplicationException("An error occurred while adding the car.");
            }
        }

        public async Task<bool> UpdateCarAsync(int ownerId, int carId, CarDto carDto)
        {
            try
            {
                var car = await carRepo.GetByIdAsync(carId);

                if (car == null)
                {
                    throw new Exception("Car not found.");
                }

                if (car.OwnerId != ownerId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to update this car.");
                }

                car = mapper.Map(carDto, car);
                await carRepo.UpdateAsync(car);

                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error updating car: {ex.Message}");
            }
        }

        public async Task<CarResponseDto> GetCarByIdAsync(int carId, int ownerId)
        {
            var car = await carRepo.GetByIdAsync(carId);
            if (car == null || car.OwnerId != ownerId)
            {
                throw new Exception("Car not found or not owned by user.");
            }

            return mapper.Map<CarResponseDto>(car);
        }

        public async Task<List<CarResponseDto>> GetUserCarsAsync(int ownerId)
        {
            var cars = await carRepo.GetByOwnerIdAsync(ownerId);
            return mapper.Map<List<CarResponseDto>>(cars);
        }

        public async Task<bool> DeleteCarAsync(int carId, int ownerId)
        {
            var car = await carRepo.GetByIdAsync(carId);
            if (car == null || car.OwnerId != ownerId)
            {
                return false;
            }

            return await carRepo.DeleteAsync(carId);
        }
    }
}
