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
public class AccountControllerTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public AccountControllerTest(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }
    [Fact]
    public async Task Register()
    {
    
        var msg = new HttpRequestMessage(HttpMethod.Post, "/api/v1.0/identity/Account/Register" );
            
        msg.Content = new StringContent(
            "{\"email\":\"test@email.ee\",\"password\":\"Kala.maja1\",\"firstname\":\"Test\",\"lastname\":\"Testing\",\"role\":\"Admin\"}",
            Encoding.UTF8, "application/json");
        
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        
        var response = await _client.SendAsync(msg);
        
        response.EnsureSuccessStatusCode();

    }

    [Fact]
    public async Task Login()
    {
        var user = "admin@eesti.ee";
        var pass = "Kala.maja1";
        
        // get jwt
        var response =
            await _client.PostAsJsonAsync("/api/v1.0/identity/Account/Login", new {email = user, password = pass});
        var contentStr = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();
        
    }
    
}