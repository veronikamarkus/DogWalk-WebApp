using System.ComponentModel.DataAnnotations;
using Base.Contracts.Domain;

namespace App.BLL.DTO;

public class Profile: IDomainEntityId
{
    public Guid Id { get; set; }
    
    public Guid AppUserId { get; set; }
    
    [MaxLength(1024)]
    public string Description { get; set; } = default!;
    
    public bool Verified { get; set; }
    
    public DateTime CreatedAt { get; set; }
}