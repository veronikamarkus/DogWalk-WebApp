using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;
using Dog = App.BLL.DTO.Dog;

namespace App.BLL.Services;

public class UsersDogService :
    BaseEntityService<App.DAL.DTO.UsersDog, App.BLL.DTO.UsersDog, IUsersDogRepository>,
    IUsersDogService
{
    public UsersDogService(IAppUnitOfWork uoW, IUsersDogRepository repository, IMapper mapper) : base(uoW,
        repository, new BllDalMapper<App.DAL.DTO.UsersDog, App.BLL.DTO.UsersDog>(mapper))
    {
    }
    
}