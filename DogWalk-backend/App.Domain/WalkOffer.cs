using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Domain;

namespace App.Domain;

public class WalkOffer: BaseEntityId, IDomainAppUser<AppUser>
{
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    
    public Guid WalkId { get; set; }
    public Walk? Walk { get; set; }
    
    [MaxLength(1024)] 
    public string Comment { get; set; } = default!;
   
    public bool Accepted { get; set; }
}