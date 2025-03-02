using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Domain;

namespace App.Domain;

public class Profile: BaseEntityId, IDomainAppUser<AppUser>
{
    [MaxLength(1024)]
    public string Description { get; set; } = default!;
    public bool Verified { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Foreign Key
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    
}