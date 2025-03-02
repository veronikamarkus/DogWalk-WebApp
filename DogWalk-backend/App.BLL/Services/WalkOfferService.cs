using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;

namespace App.BLL.Services;

public class WalkOfferService :
    BaseEntityService<App.DAL.DTO.WalkOffer, App.BLL.DTO.WalkOffer, IWalkOfferRepository>,
    IWalkOfferService
{
    public WalkOfferService(IAppUnitOfWork uoW, IWalkOfferRepository repository, IMapper mapper) : base(uoW,
        repository, new BllDalMapper<App.DAL.DTO.WalkOffer, App.BLL.DTO.WalkOffer>(mapper))
    {
    }
    
}