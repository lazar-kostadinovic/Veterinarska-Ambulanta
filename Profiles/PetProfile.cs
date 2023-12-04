using AutoMapper;
using eVeterinarskaAmbulanta.DTOs;
using eVeterinarskaAmbulanta.Entities;

namespace eVeterinarskaAmbulanta.Profiles;

public class PetProfile : Profile
{
    public PetProfile()
    {
        CreateMap<Pet, PetDto>();
        CreateMap<PetDto, Pet>();
        CreateMap<PetDtoCreate, Pet>();
        CreateMap<PetDtoUpdate, Pet>();
        CreateMap<Pet, PetWithAppDto>();
    }
}
