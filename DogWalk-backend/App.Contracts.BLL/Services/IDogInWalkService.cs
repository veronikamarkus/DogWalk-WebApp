using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IDogInWalkService: IEntityRepository<App.BLL.DTO.DogInWalk>, IDogInWalkRepositoryCustom<App.BLL.DTO.DogInWalk>
{
    
}