using AutoMapper;
using eVeterinarskaAmbulanta.DTOs;
using eVeterinarskaAmbulanta.Entities;

namespace eVeterinarskaAmbulanta.Profiles;

public class AmbulanceProfile : Profile
{
    public AmbulanceProfile()
    {
        CreateMap<Ambulance, AmbulanceDto>();
        CreateMap<AmbulanceDto, Ambulance>();
        CreateMap<AmbulanceDtoCreate, Ambulance>();
        CreateMap<AmbulanceDtoUpdate, Ambulance>();
    }

}
