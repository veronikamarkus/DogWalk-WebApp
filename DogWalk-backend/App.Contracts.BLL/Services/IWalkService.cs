using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IWalkService : IEntityRepository<App.BLL.DTO.Walk>, IWalkRepositoryCustom<App.BLL.DTO.Walk>
{
    
}