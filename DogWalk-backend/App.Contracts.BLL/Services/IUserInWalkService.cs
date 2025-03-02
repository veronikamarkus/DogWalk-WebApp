using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IUserInWalkService : IEntityRepository<App.BLL.DTO.UserInWalk>, IUserInWalkRepositoryCustom<App.BLL.DTO.UserInWalk>
{
    
}