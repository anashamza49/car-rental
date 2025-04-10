using AutoMapper;
using Authentification.JWT.DAL.Models;
using Authentification.JWT.Service.DTOs;

namespace Authentification.JWT.Service.Mappings
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<RegisterDto, Register>();
            CreateMap<LoginDto, Login>();
            // mapping Register to User and hash the psw before assigning it to PasswordHash
            CreateMap<Register, User>()
                .ForMember(dest => dest.PasswordHash,
                    opt => opt.MapFrom(src => PasswordHasher.HashPassword(src.Password)));

            CreateMap<User, UserDto>();
            CreateMap<Car, CarResponseDto>();
            CreateMap<CarDto, Car>();
        }
    }
}