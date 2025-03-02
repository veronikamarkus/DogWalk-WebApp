using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IProfileService : IEntityRepository<App.BLL.DTO.Profile>, IProfileRepositoryCustom<App.BLL.DTO.Profile>
{
    
}