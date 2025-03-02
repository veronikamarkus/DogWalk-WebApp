using Base.Contracts.DAL;
using DALDTO = App.DAL.DTO;

namespace App.Contracts.DAL.Repositories;

public interface IDogInWalkRepository : IEntityRepository<DALDTO.DogInWalk>, IDogInWalkRepositoryCustom<DALDTO.DogInWalk>
{
    // define your DAL only custom methods here
}


// define your shared (with bll) custom methods here
public interface IDogInWalkRepositoryCustom<TEntity>
{

}