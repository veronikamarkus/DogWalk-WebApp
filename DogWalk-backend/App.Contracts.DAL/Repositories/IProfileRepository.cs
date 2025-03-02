using DALDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IProfileRepository: IEntityRepository<DALDTO.Profile>, IProfileRepositoryCustom<DALDTO.Profile>
{
    // define your DAL only custom methods here
}

// define your shared (with bll) custom methods here
public interface IProfileRepositoryCustom<TEntity>
{
    Task<TEntity> FindProfileByUserId(Guid userId);
    Task<string> GetProfileDescription(Guid userId);
}