using Base.Domain;

namespace App.DAL.DTO;

public class DogInWalk: BaseEntityId
{
    public Guid WalkId { get; set; }
    public Guid DogId { get; set; }
}