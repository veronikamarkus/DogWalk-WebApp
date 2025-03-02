using App.BLL.Services;
using App.Contracts.BLL;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.DAL.EF;
using App.DAL.EF.Repositories;
using AutoMapper;
using Base.BLL;

namespace App.BLL;

public class AppBLL: BaseBLL<AppDbContext>, IAppBLL
{
    private readonly IMapper _mapper;
    private readonly IAppUnitOfWork _uow;
    
    public AppBLL(IAppUnitOfWork uoW, IMapper mapper) : base(uoW)
    {
        _mapper = mapper;
        _uow = uoW;
    }

    private IDogService? _dogs;
    public IDogService Dogs => _dogs ?? new DogService(_uow, _uow.Dogs, _mapper);
    
    private IProfileService? _profiles;
    
    public IProfileService Profiles => _profiles ?? new ProfileService(_uow, _uow.Profiles, _mapper);
    
    private IUsersDogService? _usersDogs;
    
    public IUsersDogService UsersDogs => _usersDogs ?? new UsersDogService(_uow, _uow.UsersDogs, _mapper);
    
    private IWalkService? _usersWalks;
    
    public IWalkService Walks => _usersWalks ?? new WalkService(_uow, _uow.Walks, _mapper);
    
    private IUserInWalkService? _usersInWalks;
    
    public IUserInWalkService UserInWalks => _usersInWalks ?? new UserInWalkService(_uow, _uow.UserInWalks, _mapper);
    
    private ILocationService? _locations;
    
    public ILocationService Locations => _locations ?? new LocationService(_uow, _uow.Locations, _mapper);
    
    private IDogInWalkService? _dogInWalks;
    
    public IDogInWalkService DogInWalks => _dogInWalks ?? new DogInWalkService(_uow, _uow.DogInWalks, _mapper);
    
    private IWalkOfferService? _walkOffers;
    
    public IWalkOfferService WalkOffers => _walkOffers ?? new WalkOfferService(_uow, _uow.WalkOffers, _mapper);
}