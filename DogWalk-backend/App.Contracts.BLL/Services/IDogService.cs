// ReSharper disable once RedundantUsingDirective
using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IDogService : IEntityRepository<App.BLL.DTO.Dog>, IDogRepositoryCustom<App.BLL.DTO.Dog>
{
    
}