using System.Net;

namespace App.DTO.v1_0;

public class RestApiErrorResponseDTO
{
    public HttpStatusCode Status { get; set; }
    public string Error { get; set; } = default!;

}