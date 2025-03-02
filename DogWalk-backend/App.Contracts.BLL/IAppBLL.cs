using App.Contracts.BLL.Services;
using Base.Contracts.BLL;

namespace App.Contracts.BLL;

public interface IAppBLL : IBLL
{
    IDogService Dogs { get; }
    IProfileService Profiles { get; }
    
    IUsersDogService UsersDogs { get; }
    IWalkService Walks { get; }
    IUserInWalkService UserInWalks { get; }
    ILocationService Locations { get; }
    IDogInWalkService DogInWalks { get; }
    IWalkOfferService WalkOffers { get; }
}