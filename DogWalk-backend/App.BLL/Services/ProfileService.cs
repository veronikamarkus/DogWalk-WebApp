using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;
using Profile = App.BLL.DTO.Profile;

namespace App.BLL.Services;

public class ProfileService :
    BaseEntityService<App.DAL.DTO.Profile, App.BLL.DTO.Profile, IProfileRepository>,
    IProfileService
{
    public ProfileService( IAppUnitOfWork uoW, IProfileRepository repository, IMapper mapper) : base(uoW,
        repository, new BllDalMapper<App.DAL.DTO.Profile, App.BLL.DTO.Profile>(mapper))
    {
    }

    public async Task<Profile> FindProfileByUserId(Guid userId)
    {
        return Mapper.Map(await Repository.FindProfileByUserId(userId))!;
    }

    public Task<string> GetProfileDescription(Guid userId)
    {
        return Repository.GetProfileDescription(userId);
    }
}