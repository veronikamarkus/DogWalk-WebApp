using DALDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IWalkOfferRepository: IEntityRepository<DALDTO.WalkOffer>, IWalkOfferRepositoryCustom<DALDTO.WalkOffer>
{
    
}

public interface IWalkOfferRepositoryCustom<TEntity>
{

}