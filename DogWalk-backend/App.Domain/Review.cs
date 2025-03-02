using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Domain;

namespace App.Domain;

public class Review: BaseEntityId
{
    public Guid ReviewerUserId { get; set; }
    public AppUser? ReviewerUser { get; set; }
    
    public Guid RevieweeUserId { get; set; }
    public AppUser? RevieweeUser { get; set; }
    
    public int Stars { get; set; }
    
    [MaxLength(128)] 
    public string Title { get; set; } = default!;
    
    [MaxLength(1024)] 
    public string Description { get; set; } = default!;
    
    public DateTime CreatedAt { get; set; }
}