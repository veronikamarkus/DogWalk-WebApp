using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.DAL.DTO;

public class WalkOffer: BaseEntityId
{
    public Guid AppUserId { get; set; }
    
    public Guid WalkId { get; set; }
    
    [MaxLength(1024)] 
    public string Comment { get; set; } = default!;
   
    public bool Accepted { get; set; }

}