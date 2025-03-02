using System.ComponentModel.DataAnnotations;
using Base.Contracts.Domain;

namespace App.DAL.DTO;

public class Dog: IDomainEntityId
{
    public Guid Id { get; set; }
    
    [MaxLength(128)]
    public string DogName { get; set; } = default!;
    
    public int Age { get; set; }
    
    [MaxLength(128)]
    public string Breed { get; set; } = default!;
    
    [MaxLength(1024)]
    public string Description { get; set; } = default!;
}