using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class MessageBox: BaseEntityId
{
    // UserInWalk one-to-many MessageBox
    public Guid UserInWalkId { get; set; }
    public UserInWalk? UserInWalk { get; set; }

    [MaxLength(1024)] 
    public string Message { get; set; } = default!;
    
    public DateTime CreatedAt { get; set; }
}