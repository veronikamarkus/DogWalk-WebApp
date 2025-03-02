using Base.Domain;

namespace App.DTO.v1_0;

public class DogInWalk: BaseEntityId
{
    public Guid WalkId { get; set; }
    public Guid DogId { get; set; }
}