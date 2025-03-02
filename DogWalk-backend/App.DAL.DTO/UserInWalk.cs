using Base.Contracts.Domain;

namespace App.DAL.DTO;

public class UserInWalk: IDomainEntityId, IDomainAppUserId<Guid>
{
    public Guid Id { get; set; }
    public Guid AppUserId { get; set; }
    public Guid WalkId { get; set; }
}