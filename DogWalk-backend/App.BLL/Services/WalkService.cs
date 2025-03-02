using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;

namespace App.BLL.Services;

public class WalkService :
    BaseEntityService<App.DAL.DTO.Walk, App.BLL.DTO.Walk, IWalkRepository>,
    IWalkService
{
    public WalkService(IAppUnitOfWork uoW, IWalkRepository repository, IMapper mapper) : base(uoW,
        repository, new BllDalMapper<App.DAL.DTO.Walk, App.BLL.DTO.Walk>(mapper))
    {
    }
    
    public async Task<IEnumerable<App.BLL.DTO.Walk>> GetActiveWalksByLocation(string location)
    {
        return (await Repository.GetActiveWalksByLocation(location)).Select(w => Mapper.Map(w));
    }
}