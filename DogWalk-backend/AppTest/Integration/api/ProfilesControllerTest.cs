using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using App.DTO.v1_0.Identity;
using Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using NuGet.Protocol.Core.Types;
using WebApp.Helpers;

namespace App.Test.Integration.api;

[Collection("NonParallel")]
public class ProfilesControllerTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public ProfilesControllerTest(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }
    
    [Fact]
    public async Task AddProfile()
    {
        var user = "admin@eesti.ee";
        var pass = "Kala.maja1";
    
        // get jwt
        var response =
            await _client.PostAsJsonAsync("/api/v1.0/identity/Account/Login", new {email = user, password = pass});
        var contentStr = await response.Content.ReadAsStringAsync();
        // response.EnsureSuccessStatusCode();
    
        var loginData = JsonSerializer.Deserialize<JWTResponseDTO>(contentStr, JsonHelper.CamelCase);
        
        //add profile
        
        var profileId = Guid.NewGuid().ToString();
        
        var msg1 = new HttpRequestMessage(HttpMethod.Post, "/api/v1.0/Profiles" );
        
        msg1.Content = new StringContent(
            "{\"id\":\"" + profileId +
            "\",\"description\":\"Test Profile\",\"verified\":true,\"createdAt\":\"2024-05-23T17:33:05\"}",
            Encoding.UTF8, "application/json");
            
        msg1.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData!.Jwt);
        msg1.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    
        response = await _client.SendAsync(msg1);
    
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetProfile()
    {
        var user = "admin@eesti.ee";
        var pass = "Kala.maja1";
    
        // get jwt
        var response =
            await _client.PostAsJsonAsync("/api/v1.0/identity/Account/Login", new {email = user, password = pass});
        var contentStr = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();
    
        var loginData = JsonSerializer.Deserialize<JWTResponseDTO>(contentStr, JsonHelper.CamelCase);
        
        //add profile
        
        var profileId = Guid.NewGuid().ToString();
        
        var msg1 = new HttpRequestMessage(HttpMethod.Post, "/api/v1.0/Profiles" );
        
        msg1.Content = new StringContent(
            "{\"id\":\"" + profileId +
            "\",\"description\":\"Test Profile\",\"verified\":true,\"createdAt\":\"2024-05-23T17:33:05\"}",
            Encoding.UTF8, "application/json");
            
        msg1.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData!.Jwt);
        msg1.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    
        response = await _client.SendAsync(msg1);
    
        response.EnsureSuccessStatusCode();
    
        // get profile
        
        var msg = new HttpRequestMessage(HttpMethod.Get, "/api/v1.0/Profiles/" + profileId);
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Jwt);
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    
        var response2 = await _client.SendAsync(msg);
    
        response2.EnsureSuccessStatusCode();
    }
    
}