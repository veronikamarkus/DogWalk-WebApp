using App.Contracts.DAL.Repositories;
using App.Domain;
using AutoMapper;
using Base.Contracts.DAL;
using Base.DAL.EF;
using DALDTO = App.DAL.DTO;
using APPDomain = App.Domain;

namespace App.DAL.EF.Repositories;

public class WalkOfferRepository: BaseEntityRepository<APPDomain.WalkOffer, DALDTO.WalkOffer, AppDbContext>, IWalkOfferRepository
{
    public WalkOfferRepository(AppDbContext dbContext, IMapper mapper) :
        base(dbContext, new DalDomainMapper<APPDomain.WalkOffer, DALDTO.WalkOffer>(mapper))
    {
    }
}