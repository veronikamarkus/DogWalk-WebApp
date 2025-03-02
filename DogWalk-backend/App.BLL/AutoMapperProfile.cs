using AutoMapper;

namespace App.BLL;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<App.DAL.DTO.Dog, App.BLL.DTO.Dog>().ReverseMap();
        CreateMap<App.DAL.DTO.Profile, App.BLL.DTO.Profile>().ReverseMap();
        CreateMap<App.DAL.DTO.UsersDog, App.BLL.DTO.UsersDog>().ReverseMap();
        CreateMap<App.DAL.DTO.Walk, App.BLL.DTO.Walk>().ReverseMap();
        CreateMap<App.DAL.DTO.UserInWalk, App.BLL.DTO.UserInWalk>().ReverseMap();
        CreateMap<App.DAL.DTO.DogInWalk, App.BLL.DTO.DogInWalk>().ReverseMap();
        CreateMap<App.DAL.DTO.Location, App.BLL.DTO.Location>().ReverseMap();
        CreateMap<App.DAL.DTO.WalkOffer, App.BLL.DTO.WalkOffer>().ReverseMap();
    }
}