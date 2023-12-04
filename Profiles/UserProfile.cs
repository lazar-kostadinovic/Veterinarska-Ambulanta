using AutoMapper;
using eVeterinarskaAmbulanta.DTOs;
using eVeterinarskaAmbulanta.Entities;

namespace eVeterinarskaAmbulanta.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();
        CreateMap<UserDtoCreate, User>();
        CreateMap<UserDtoUpdate, User>();
        CreateMap<User, UserWithPetsDto>();
    }
}
