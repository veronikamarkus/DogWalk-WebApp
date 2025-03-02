using Base.Domain;

namespace App.Domain;

public class DogInWalk: BaseEntityId
{
    public Guid WalkId { get; set; }
    public Walk? Walk { get; set; }
    
    public Guid DogId { get; set; }
    public Dog? Dog { get; set; }
}