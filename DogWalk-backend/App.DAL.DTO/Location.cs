using System.ComponentModel.DataAnnotations;
using Base.Contracts.Domain;

namespace App.DAL.DTO;

public class Location: IDomainEntityId
{
    public Guid Id { get; set; }
    
    [MaxLength(128)]
    public string City { get; set; } = default!;
    
    [MaxLength(128)]
    public string District { get; set; } = default!;
    
    [MaxLength(128)]
    public string StartingAddress { get; set; } = default!;
    
}