using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IWalkRepository: IEntityRepository<App.DAL.DTO.Walk>, IWalkRepositoryCustom<App.DAL.DTO.Walk>
{
    
}

public interface IWalkRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetActiveWalksByLocation(string location);
}