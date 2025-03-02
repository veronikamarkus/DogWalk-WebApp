using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IWalkOfferService: IEntityRepository<App.BLL.DTO.WalkOffer>, IWalkOfferRepositoryCustom<App.BLL.DTO.WalkOffer>
{
    
}