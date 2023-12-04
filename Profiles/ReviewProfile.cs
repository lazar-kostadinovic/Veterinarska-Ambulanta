using AutoMapper;
using eVeterinarskaAmbulanta.DTOs;
using eVeterinarskaAmbulanta.Entities;

namespace eVeterinarskaAmbulanta.Profiles;

public class ReviewProfile : Profile
{
    public ReviewProfile()
    {
        CreateMap<Review, ReviewDto>();
        CreateMap<ReviewDto, Review>();
        CreateMap<ReviewDtoCreate, Review>();
        CreateMap<ReviewDtoUpdate, Review>();
    }

}
