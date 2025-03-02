using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Domain;

namespace App.Domain;

public class Walk: BaseEntityId
{
    // Walk many-to-many Dog
   // public ICollection<Dog> Dogs { get; set; } = new List<Dog>();
    
    // Walk one-to-many UsersInWalk
    public ICollection<UserInWalk> UsersInWalk { get; set; } = new List<UserInWalk>();
    
    // Walk one-to-many WalkOffer
    public ICollection<WalkOffer> WalkOffers { get; set; } = new List<WalkOffer>();
    
    // Walk one-to-many DogInWalk
    public ICollection<DogInWalk> DogsInWalk { get; set; } = new List<DogInWalk>();
    
    // FK
    public Guid LocationId { get; set; }
    public Location? Location { get; set; }
    
    public DateTime TargetStartingTime { get; set; }
    public int TargetDurationMinutes { get; set; }
    public decimal Price { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public bool Closed { get; set; }
    [MaxLength(1024)] 
    public string Description { get; set; } = default!;
    
}