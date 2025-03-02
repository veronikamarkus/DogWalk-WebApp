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
public class DogsControllerTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public DogsControllerTest(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task IndexRequiresLogin()
    {
        // Act
        var response = await _client.GetAsync("/api/v1.0/Dogs");
        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task IndexWithUser()
    {
        var user = "admin@eesti.ee";
        var pass = "Kala.maja1";
    
        // get jwt
        var response =
            await _client.PostAsJsonAsync("/api/v1.0/identity/Account/Login", new {email = user, password = pass});
        var contentStr = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();
    
        var loginData = JsonSerializer.Deserialize<JWTResponseDTO>(contentStr, JsonHelper.CamelCase);
    
        Assert.NotNull(loginData);
        Assert.NotNull(loginData.Jwt);
        Assert.True(loginData.Jwt.Length > 0);
    
        var msg = new HttpRequestMessage(HttpMethod.Get, "/api/v1.0/Dogs");
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Jwt);
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    
        response = await _client.SendAsync(msg);
    
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task AddDog()
    {
        var user = "admin@eesti.ee";
        var pass = "Kala.maja1";
    
        // get jwt
        var response =
            await _client.PostAsJsonAsync("/api/v1.0/identity/Account/Login", new {email = user, password = pass});
        var contentStr = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();
    
        var loginData = JsonSerializer.Deserialize<JWTResponseDTO>(contentStr, JsonHelper.CamelCase);
    
        Assert.NotNull(loginData);
        Assert.NotNull(loginData.Jwt);
        Assert.True(loginData.Jwt.Length > 0);
    
        // create dog

        var dogId = Guid.NewGuid().ToString();
        
        var msg = new HttpRequestMessage(HttpMethod.Post, "/api/v1.0/Dogs" );
        
        msg.Content = new StringContent(
            "{\"id\":\"" + dogId +
            "\",\"dogName\":\"Test Dog\",\"age\":3,\"breed\":\"TestDog\",\"description\":\"to test create dog api\"}",
            Encoding.UTF8, "application/json");
            
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Jwt);
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    
        response = await _client.SendAsync(msg);
    
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task DeleteDog()
    {
        var user = "admin@eesti.ee";
        var pass = "Kala.maja1";
    
        // get jwt
        var response =
            await _client.PostAsJsonAsync("/api/v1.0/identity/Account/Login", new {email = user, password = pass});
        var contentStr = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();
    
        var loginData = JsonSerializer.Deserialize<JWTResponseDTO>(contentStr, JsonHelper.CamelCase);
    
        Assert.NotNull(loginData);
        Assert.NotNull(loginData.Jwt);
        Assert.True(loginData.Jwt.Length > 0);
        
        // add dog

        var dogId = Guid.NewGuid().ToString();
        
        var msg1 = new HttpRequestMessage(HttpMethod.Post, "/api/v1.0/Dogs" );
        
        msg1.Content = new StringContent(
            "{\"id\":\"" + dogId +
            "\",\"dogName\":\"Test Dog\",\"age\":3,\"breed\":\"TestDog\",\"description\":\"to test create dog api\"}",
            Encoding.UTF8, "application/json");
            
        msg1.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Jwt);
        msg1.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    
        var response1 = await _client.SendAsync(msg1);
        
        response1.EnsureSuccessStatusCode();
        
        // delete dog
    
        var msg2 = new HttpRequestMessage(HttpMethod.Delete, "/api/v1.0/Dogs/" + dogId);
        msg2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Jwt);
        msg2.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    
        response = await _client.SendAsync(msg2);
    
        response.EnsureSuccessStatusCode();
    }
    
}