using Base.Contracts.Domain;

namespace App.DTO.v1_0;

public class UsersDog: IDomainEntityId, IDomainAppUserId<Guid>
{
    public Guid Id { get; set; }
    public Guid AppUserId { get; set; }
    public Guid DogId { get; set; }
}