using System.Text.Json;

namespace WebApp.Helpers;

public static class JsonHelper
{
    public static JsonSerializerOptions CamelCase = new JsonSerializerOptions()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
}