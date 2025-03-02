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
public class WalkOffersControllerTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public WalkOffersControllerTest(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }
    
    [Fact]
    public async Task AddWalkOffer()
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
        
        var msg0 = new HttpRequestMessage(HttpMethod.Post, "/api/v1.0/Dogs" );
        
        msg0.Content = new StringContent(
            "{\"id\":\"" + dogId +
            "\",\"dogName\":\"Test Dog\",\"age\":3,\"breed\":\"TestDog\",\"description\":\"to test create dog api\"}",
            Encoding.UTF8, "application/json");
            
        msg0.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Jwt);
        msg0.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    
        response = await _client.SendAsync(msg0);
    
        response.EnsureSuccessStatusCode();
        
        //add location
        var locationId = Guid.NewGuid().ToString();
        var msg1 = new HttpRequestMessage(HttpMethod.Post, "/api/v1.0/Locations" );
        
        msg1.Content = new StringContent(
            "{\"id\":\"" + locationId +
            "\",\"city\":\"Tallinn\",\"district\":\"Kopli\",\"startingAddress\":\"Test street 32\"}",
            Encoding.UTF8, "application/json");
            
        msg1.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Jwt);
        msg1.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    
        var response1 = await _client.SendAsync(msg1);
    
        response1.EnsureSuccessStatusCode();
        
        // add walk

        var walkId = Guid.NewGuid().ToString();
        var msg = new HttpRequestMessage(HttpMethod.Post, "/api/v1.0/Walks/" + dogId );
        
        msg.Content = new StringContent(
            "{\"id\":\"" + walkId +
            "\",\"locationId\":\"" + locationId +
            "\",\"targetStartingTime\":\"2024-05-23T18:36:43\",\"targetDurationMinutes\":60,\"price\":23,\"startedAt\":\"2024-05-23T18:36:43\",\"finishedAt\":\"2024-05-23T18:36:43\",\"closed\":false,\"description\":\"walk test\"}",
            Encoding.UTF8, "application/json");
            
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Jwt);
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    
        var response2 = await _client.SendAsync(msg);
    
        response2.EnsureSuccessStatusCode();
        
        // add walk offer
        
        var walkOfferId = Guid.NewGuid().ToString();
        var appUserId = "00000000-0000-0000-0000-000000000000"; //Guid will be created at api controller
        var msg3 = new HttpRequestMessage(HttpMethod.Post, "/api/v1.0/WalkOffers" );
        
        msg3.Content = new StringContent(
            "{\"id\":\"" + walkOfferId +
            "\",\"appUserId\":\"" + appUserId +
            "\",\"walkId\":\"" + walkId + 
            "\",\"comment\":\"test\",\"accepted\":true}",
            Encoding.UTF8, "application/json");
            
        msg3.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Jwt);
        msg3.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    
        var response3 = await _client.SendAsync(msg3);
    
        response3.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task UpdateWalkOffer()
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
        
        var msg0 = new HttpRequestMessage(HttpMethod.Post, "/api/v1.0/Dogs" );
        
        msg0.Content = new StringContent(
            "{\"id\":\"" + dogId +
            "\",\"dogName\":\"Test Dog\",\"age\":3,\"breed\":\"TestDog\",\"description\":\"to test create dog api\"}",
            Encoding.UTF8, "application/json");
            
        msg0.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Jwt);
        msg0.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    
        response = await _client.SendAsync(msg0);
    
        response.EnsureSuccessStatusCode();
        
        //add location
        var locationId = Guid.NewGuid().ToString();
        var msg1 = new HttpRequestMessage(HttpMethod.Post, "/api/v1.0/Locations" );
        
        msg1.Content = new StringContent(
            "{\"id\":\"" + locationId +
            "\",\"city\":\"Tallinn\",\"district\":\"Kopli\",\"startingAddress\":\"Test street 32\"}",
            Encoding.UTF8, "application/json");
            
        msg1.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Jwt);
        msg1.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    
        var response1 = await _client.SendAsync(msg1);
    
        response1.EnsureSuccessStatusCode();
        
        // add walk

        var walkId = Guid.NewGuid().ToString();
        var msg = new HttpRequestMessage(HttpMethod.Post, "/api/v1.0/Walks/" + dogId );
        
        msg.Content = new StringContent(
            "{\"id\":\"" + walkId +
            "\",\"locationId\":\"" + locationId +
            "\",\"targetStartingTime\":\"2024-05-23T18:36:43\",\"targetDurationMinutes\":60,\"price\":23,\"startedAt\":\"2024-05-23T18:36:43\",\"finishedAt\":\"2024-05-23T18:36:43\",\"closed\":false,\"description\":\"walk test\"}",
            Encoding.UTF8, "application/json");
            
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Jwt);
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    
        var response2 = await _client.SendAsync(msg);
    
        response2.EnsureSuccessStatusCode();
        
        // add walk offer
        
        var walkOfferId = Guid.NewGuid().ToString();
        var appUserId = "00000000-0000-0000-0000-000000000000"; //Guid will be created at api controller
        var msg3 = new HttpRequestMessage(HttpMethod.Post, "/api/v1.0/WalkOffers" );
        
        msg3.Content = new StringContent(
            "{\"id\":\"" + walkOfferId +
            "\",\"appUserId\":\"" + appUserId +
            "\",\"walkId\":\"" + walkId + 
            "\",\"comment\":\"test\",\"accepted\":false}",
            Encoding.UTF8, "application/json");
            
        msg3.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Jwt);
        msg3.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    
        var response3 = await _client.SendAsync(msg3);
    
        response3.EnsureSuccessStatusCode();
        
        // update walk offer
        var msg4 = new HttpRequestMessage(HttpMethod.Put, "/api/v1.0/WalkOffers/" + walkOfferId );
        
        msg4.Content = new StringContent(
            "{\"id\":\"" + walkOfferId +
            "\",\"appUserId\":\"" + appUserId +
            "\",\"walkId\":\"" + walkId + 
            "\",\"comment\":\"test\",\"accepted\":true}",
            Encoding.UTF8, "application/json");
        
        msg4.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Jwt);
        msg4.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    
        var response4 = await _client.SendAsync(msg4);
    
        response3.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task DeleteWalkOffer()
    {
        var user = "admin@eesti.ee";
        var pass = "Kala.maja1";
    
        // get jwt
        var response = await _client.PostAsJsonAsync("/api/v1.0/identity/Account/Login", new {email = user, password = pass});
        var contentStr = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();
    
        var loginData = JsonSerializer.Deserialize<JWTResponseDTO>(contentStr, JsonHelper.CamelCase);
    
        Assert.NotNull(loginData);
        Assert.NotNull(loginData.Jwt);
        Assert.True(loginData.Jwt.Length > 0);
        // add dog
        
        var dogId = Guid.NewGuid().ToString();
        
        var msg0 = new HttpRequestMessage(HttpMethod.Post, "/api/v1.0/Dogs" );
        
        msg0.Content = new StringContent(
            "{\"id\":\"" + dogId +
            "\",\"dogName\":\"Test Dog\",\"age\":3,\"breed\":\"TestDog\",\"description\":\"to test create dog api\"}",
            Encoding.UTF8, "application/json");
            
        msg0.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Jwt);
        msg0.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    
        response = await _client.SendAsync(msg0);
    
        response.EnsureSuccessStatusCode();
        
        //add location
        var locationId = Guid.NewGuid().ToString();
        var msg1 = new HttpRequestMessage(HttpMethod.Post, "/api/v1.0/Locations" );
        
        msg1.Content = new StringContent(
            "{\"id\":\"" + locationId +
            "\",\"city\":\"Tallinn\",\"district\":\"Kopli\",\"startingAddress\":\"Test street 32\"}",
            Encoding.UTF8, "application/json");
            
        msg1.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Jwt);
        msg1.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    
        var response1 = await _client.SendAsync(msg1);
    
        response1.EnsureSuccessStatusCode();
        
        // add walk

        var walkId = Guid.NewGuid().ToString();
        var msg = new HttpRequestMessage(HttpMethod.Post, "/api/v1.0/Walks/" + dogId );
        
        msg.Content = new StringContent(
            "{\"id\":\"" + walkId +
            "\",\"locationId\":\"" + locationId +
            "\",\"targetStartingTime\":\"2024-05-23T18:36:43\",\"targetDurationMinutes\":60,\"price\":23,\"startedAt\":\"2024-05-23T18:36:43\",\"finishedAt\":\"2024-05-23T18:36:43\",\"closed\":false,\"description\":\"walk test\"}",
            Encoding.UTF8, "application/json");
            
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Jwt);
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    
        var response2 = await _client.SendAsync(msg);
    
        response2.EnsureSuccessStatusCode();
        
        // add walk offer
        
        var walkOfferId = Guid.NewGuid().ToString();
        var appUserId = "00000000-0000-0000-0000-000000000000"; //Guid will be created at api controller
        var msg3 = new HttpRequestMessage(HttpMethod.Post, "/api/v1.0/WalkOffers" );
        
        msg3.Content = new StringContent(
            "{\"id\":\"" + walkOfferId +
            "\",\"appUserId\":\"" + appUserId +
            "\",\"walkId\":\"" + walkId + 
            "\",\"comment\":\"test\",\"accepted\":false}",
            Encoding.UTF8, "application/json");
            
        msg3.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Jwt);
        msg3.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    
        var response3 = await _client.SendAsync(msg3);
    
        response3.EnsureSuccessStatusCode();
        
        // delete walk offer
        var msg4 = new HttpRequestMessage(HttpMethod.Delete, "/api/v1.0/WalkOffers/" + walkOfferId );
        
        msg4.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Jwt);
        msg4.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    
        var response4 = await _client.SendAsync(msg4);
    
        response3.EnsureSuccessStatusCode();
    }
    
}