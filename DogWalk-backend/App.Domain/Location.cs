using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class Location: BaseEntityId
{
    [MaxLength(128)]
    public string City { get; set; } = default!;
    [MaxLength(128)]
    public string District { get; set; } = default!;
    [MaxLength(128)]
    public string StartingAddress { get; set; } = default!;
    
    // Location one-to-many Walk
    public ICollection<Walk> Walks { get; set; } = new List<Walk>();
}