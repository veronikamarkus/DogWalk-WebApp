using DALDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface ILocationRepository: IEntityRepository<DALDTO.Location>, ILocationRepositoryCustom<DALDTO.Location>
{
    // define your DAL only custom methods here
}

// define your shared (with bll) custom methods here
public interface ILocationRepositoryCustom<TEntity>
{
        
}