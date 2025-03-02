using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface ILocationService: IEntityRepository<App.BLL.DTO.Location>, ILocationRepositoryCustom<App.BLL.DTO.Location>
{
    
}