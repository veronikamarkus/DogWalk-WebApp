// ReSharper disable once RedundantUsingDirective
using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IUsersDogService : IEntityRepository<App.BLL.DTO.UsersDog>, IUsersDogRepositoryCustom<App.BLL.DTO.UsersDog>
{
    
}