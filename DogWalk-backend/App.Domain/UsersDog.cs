using App.Domain.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.Domain;

public class UsersDog: BaseEntityId, IDomainAppUserId<Guid>
{
    public Guid DogId { get; set; }
    public Dog? Dog { get; set; }
    
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}