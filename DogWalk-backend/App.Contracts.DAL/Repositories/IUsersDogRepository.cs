using DALDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IUsersDogRepository : IEntityRepository<DALDTO.UsersDog>, IUsersDogRepositoryCustom<DALDTO.UsersDog>
{
    // define your DAL only custom methods here
}


// define your shared (with bll) custom methods here
public interface IUsersDogRepositoryCustom<TEntity>
{

}