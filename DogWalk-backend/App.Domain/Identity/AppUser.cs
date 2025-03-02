using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Contracts.Domain;
using Microsoft.AspNetCore.Identity;

namespace App.Domain.Identity;

public class AppUser: IdentityUser<Guid>, IDomainEntityId
{
    [MinLength(1)]
    [MaxLength(64)]
    public string FirstName { get; set; } = default!;
    
    [MinLength(1)]
    [MaxLength(64)]
    public string LastName { get; set; } = default!;
    // 1:0-1, Fk in dependent entity
    public Profile? Profile { get; set; }
    
    // User one-to-many Dogs. Collection navigation property
    //public ICollection<Dog> Dogs { get; set; } = default!;
    
    public ICollection<UsersDog> UsersDogs { get; set; } = new List<UsersDog>();
    
    [InverseProperty("ReviewerUser")]
    public List<Review> ReviewsAsReviewer { get; set; } = default!;
    [InverseProperty("RevieweeUser")]
    public List<Review> ReviewsAsReviewee { get; set; } = default!;
    
    public ICollection<WalkOffer> WalkOffers { get; set; } = default!;
    
    public ICollection<AppRefreshToken>? RefreshTokens { get; set; }
}