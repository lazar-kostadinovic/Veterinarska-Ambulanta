using AutoMapper;
using eVeterinarskaAmbulanta.DTOs;
using eVeterinarskaAmbulanta.Entities;

namespace eVeterinarskaAmbulanta.Profiles;

public class VeterinarianProfile : Profile
{
    public VeterinarianProfile()
    {
        CreateMap<Veterinarian, VeterinarianDto>();
        CreateMap<VeterinarianDto, Veterinarian>();
        CreateMap<VeterinarianDtoCreate, Veterinarian>();
        CreateMap<VeterinarianDtoUpdate, Veterinarian>();
    }

}

