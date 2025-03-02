namespace App.DTO.v1_0.Identity;

public class JWTResponseDTO
{
    public string Jwt { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
    public string Role { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
}