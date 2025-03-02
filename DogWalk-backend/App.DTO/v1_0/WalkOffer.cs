using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Domain;

namespace App.DTO.v1_0;

public class WalkOffer: BaseEntityId
{
    public Guid AppUserId { get; set; }
    
    public Guid WalkId { get; set; }
    
    [MaxLength(1024)] 
    public string Comment { get; set; } = default!;
   
    public bool Accepted { get; set; }

}