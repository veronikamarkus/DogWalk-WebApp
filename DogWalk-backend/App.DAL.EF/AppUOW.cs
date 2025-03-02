using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using App.DAL.EF.Repositories;
using App.Domain.Identity;
using AutoMapper;
using Base.Contracts.DAL;
using Base.DAL.EF;

namespace App.DAL.EF;

public class AppUOW : BaseUnitOfWork<AppDbContext>, IAppUnitOfWork
{
    private readonly IMapper _mapper;
    public AppUOW(AppDbContext dbContext, IMapper mapper) : base(dbContext)
    {
        _mapper = mapper;
    }

    private IProfileRepository? _profiles;
    public IProfileRepository Profiles => _profiles ?? new  ProfileRepository(UowDbContext, _mapper);

    private IEntityRepository<AppUser>? _users;
    
    public IEntityRepository<AppUser> Users => _users ??
                                               new BaseEntityRepository<AppUser, AppUser, AppDbContext>(UowDbContext,
                                                   new DalDomainMapper<AppUser, AppUser>(_mapper));

    public IDogRepository? _dogs;
    
    public IDogRepository Dogs => _dogs ?? new DogRepository(UowDbContext, _mapper);
    
    public ILocationRepository? _locations;
    
    public ILocationRepository Locations => _locations ?? new LocationRepository(UowDbContext, _mapper);
    
    public IUsersDogRepository? _usersDogs;
    
    public IUsersDogRepository UsersDogs => _usersDogs ?? new UsersDogRepository(UowDbContext, _mapper);
    
    public IWalkRepository? _usersWalks;
    
    public IWalkRepository Walks => _usersWalks ?? new WalkRepository(UowDbContext, _mapper);

    public IUserInWalkRepository? _userInWalks;
    
    public IUserInWalkRepository UserInWalks => _userInWalks ?? new UserInWalkRepository(UowDbContext, _mapper);
    
    public IDogInWalkRepository? _dogInWalks;
    
    public IDogInWalkRepository DogInWalks => _dogInWalks ?? new DogInWalkRepository(UowDbContext, _mapper);
    
    public IWalkOfferRepository? _walkOffers;
    
    public IWalkOfferRepository WalkOffers => _walkOffers ?? new WalkOfferRepository(UowDbContext, _mapper);
}
