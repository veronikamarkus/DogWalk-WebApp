using System.Collections;
using App.Domain.Identity;
using Base.Domain;

namespace App.Domain;

public class UserInWalk: BaseEntityId, IDomainAppUser<AppUser>
{
    public Guid WalkId { get; set; }
    public Walk? Walk { get; set; }
    
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }

    // UserInWalk one-to-many MessageBox
    public ICollection<MessageBox> MessageBoxes { get; set; } = new List<MessageBox>();
}