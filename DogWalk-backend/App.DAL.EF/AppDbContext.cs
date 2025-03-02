using App.Domain;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF;

public class AppDbContext: IdentityDbContext<AppUser, AppRole, Guid, IdentityUserClaim<Guid>,  AppUserRole,
    IdentityUserLogin<Guid>,  IdentityRoleClaim<Guid>,  IdentityUserToken<Guid>>
{
    public DbSet<AppRefreshToken> RefreshTokens { get; set; } = default!;
    
    public DbSet<Profile> Profiles { get; set; } = default!;
    public DbSet<Dog> Dogs { get; set; } = default!;
    public DbSet<Walk> Walks { get; set; } = default!;
    public DbSet<UserInWalk> UserInWalks { get; set; } = default!;
    public DbSet<DogInWalk> DogInWalks { get; set; } = default!;
    public DbSet<UsersDog> UsersDogs { get; set; } = default!;
    public DbSet<WalkOffer> WalkOffers { get; set; } = default!;
    public DbSet<Location> Locations { get; set; } = default!;
    public DbSet<Review> Reviews { get; set; } = default!;
    public DbSet<MessageBox> MessageBoxes { get; set; } = default!;
    
    public AppDbContext(DbContextOptions options): base(options)
    {
    }
    
}