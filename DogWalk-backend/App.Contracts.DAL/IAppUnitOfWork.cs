using App.Contracts.DAL.Repositories;
using App.Domain.Identity;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IAppUnitOfWork : IUnitOfWork
{
    // list your repos here

    IProfileRepository Profiles { get; }
    IEntityRepository<AppUser> Users { get; }
    IDogRepository Dogs { get; }
    ILocationRepository Locations { get; }
    IUsersDogRepository UsersDogs { get; }
    IWalkRepository Walks { get; }
    IUserInWalkRepository UserInWalks { get; }
    IDogInWalkRepository DogInWalks { get; }
    IWalkOfferRepository WalkOffers { get; }
}