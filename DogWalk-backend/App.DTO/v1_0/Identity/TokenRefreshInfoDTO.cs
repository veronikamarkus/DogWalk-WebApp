namespace App.DTO.v1_0.Identity;

public class TokenRefreshInfoDTO
{
    public string Jwt { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}