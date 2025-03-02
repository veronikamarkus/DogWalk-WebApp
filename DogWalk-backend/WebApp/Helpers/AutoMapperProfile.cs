using AutoMapper;

namespace WebApp.Helpers;

/// <summary>
/// mapper
/// </summary>
public class AutoMapperProfile: Profile
{
    /// <summary>
    /// mapper
    /// </summary>
    public AutoMapperProfile()
    {
        CreateMap<App.BLL.DTO.Profile, App.DTO.v1_0.Profile>().ReverseMap();
        CreateMap<App.BLL.DTO.Dog, App.DTO.v1_0.Dog>().ReverseMap();
        CreateMap<App.BLL.DTO.WalkOffer, App.DTO.v1_0.WalkOffer>().ReverseMap();
        CreateMap<App.BLL.DTO.Walk, App.DTO.v1_0.Walk>().ReverseMap();
        CreateMap<App.BLL.DTO.Location, App.DTO.v1_0.Location>().ReverseMap();
    }
}