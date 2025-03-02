using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;

namespace App.BLL.Services;

public class DogInWalkService:
        BaseEntityService<App.DAL.DTO.DogInWalk, App.BLL.DTO.DogInWalk, IDogInWalkRepository>,
        IDogInWalkService
{
    public DogInWalkService(IAppUnitOfWork uoW, IDogInWalkRepository repository, IMapper mapper) : base(uoW,
        repository, new BllDalMapper<App.DAL.DTO.DogInWalk, App.BLL.DTO.DogInWalk>(mapper))
    {
    }
    
}