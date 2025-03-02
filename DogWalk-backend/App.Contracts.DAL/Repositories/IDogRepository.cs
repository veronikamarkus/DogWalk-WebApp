using DALDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IDogRepository : IEntityRepository<DALDTO.Dog>, IDogRepositoryCustom<DALDTO.Dog>
{
    // define your DAL only custom methods here
}


// define your shared (with bll) custom methods here
public interface IDogRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllSortedAsync(Guid userId);
}