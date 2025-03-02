using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Domain;

namespace App.Domain;

public class Dog: BaseEntityId
{
    [MaxLength(128)]
    public string DogName { get; set; } = default!;
    public int Age { get; set; }
    [MaxLength(128)]
    public string Breed { get; set; } = default!;
    [MaxLength(1024)]
    public string Description { get; set; } = default!;
    
    // FK
    // public Guid AppUserId { get; set; }
    // public AppUser? AppUser { get; set; }
    
    public ICollection<UsersDog> UsersDogs { get; set; } = new List<UsersDog>();
    
    // Walk one-to-many DogInWalk
    public ICollection<DogInWalk> DogsInWalk { get; set; } = new List<DogInWalk>();
    
    // Dog many-to-many Walk
   // public ICollection<Walk> Walks { get; set; } = new List<Walk>();
}