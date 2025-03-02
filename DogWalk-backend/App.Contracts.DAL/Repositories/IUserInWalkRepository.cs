using App.Domain;
using Base.Contracts.DAL;
using DALDTO = App.DAL.DTO;

namespace App.Contracts.DAL.Repositories;

public interface IUserInWalkRepository: IEntityRepository<DALDTO.UserInWalk>, IUserInWalkRepositoryCustom<DALDTO.UserInWalk>
{
    
}

public interface IUserInWalkRepositoryCustom<TEntity>
{
    
}