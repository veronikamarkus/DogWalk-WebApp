using AutoMapper;

namespace App.DAL.EF;


public class AutoMapperProfile: Profile
{
    public AutoMapperProfile()
    {
        CreateMap<App.Domain.Profile, App.DAL.DTO.Profile>().ReverseMap();
        CreateMap<App.Domain.Dog, App.DAL.DTO.Dog>().ReverseMap();
        CreateMap<App.Domain.Location, App.DAL.DTO.Location>().ReverseMap();
        CreateMap<App.DAL.DTO.UsersDog, App.Domain.UsersDog>().ReverseMap();
        CreateMap<App.DAL.DTO.Walk, App.Domain.Walk>().ReverseMap();
        CreateMap<App.DAL.DTO.UserInWalk, App.Domain.UserInWalk>().ReverseMap();
        CreateMap<App.DAL.DTO.DogInWalk, App.Domain.DogInWalk>().ReverseMap();
        CreateMap<App.DAL.DTO.WalkOffer, App.Domain.WalkOffer>().ReverseMap();
    }
}