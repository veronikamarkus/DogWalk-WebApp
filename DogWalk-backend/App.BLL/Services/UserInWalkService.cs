using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;

namespace App.BLL.Services;

public class UserInWalkService :
    BaseEntityService<App.DAL.DTO.UserInWalk, App.BLL.DTO.UserInWalk, IUserInWalkRepository>,
    IUserInWalkService
{
    public UserInWalkService(IAppUnitOfWork uoW, IUserInWalkRepository repository, IMapper mapper) : base(uoW,
        repository, new BllDalMapper<App.DAL.DTO.UserInWalk, App.BLL.DTO.UserInWalk>(mapper))
    {
    }
    
}