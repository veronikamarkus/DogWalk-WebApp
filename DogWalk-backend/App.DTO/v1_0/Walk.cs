using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.DTO.v1_0;

public class Walk: BaseEntityId
{
    public Guid LocationId { get; set; }
    public DateTime TargetStartingTime { get; set; }
    public int TargetDurationMinutes { get; set; }
    public decimal Price { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public bool Closed { get; set; }
    [MaxLength(1024)] 
    public string Description { get; set; } = default!;
}