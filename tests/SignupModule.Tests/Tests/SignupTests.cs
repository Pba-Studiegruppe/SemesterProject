
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Signup_Infrastructure.Data;
using static SignupModule.Tests.Constants;

namespace SignupModule.Tests.Tests;

[Collection("Database collection")]

public class SignupTests 
{
    private readonly HttpClient _client;
    private readonly SignupDbContext _dbContext;

    public SignupTests(PostgresDbFixture dbFixture)
    {

        var factory = new SignupModuleWebApplicationFactory
        {
            ConnectionString = dbFixture.ConnectionString
        };

        _client = factory.CreateClient();

        var scope = factory.Services.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<SignupDbContext>();
    }

    [Fact]
    public async Task WhenUserSignsup_ThenSignupRequestIsSavedInDb()
    {
        // When
        var request = new
        {
            FirstName = "Mike",
            LastName = "Ox",
            Email = "mike@ox.com",
            Password = "superSecure123"
        };
        var response = await _client.PostAsJsonAsync(RouteConstants.SignupRoute, request);
        response.EnsureSuccessStatusCode();

        // Then
        Assert.Single(_dbContext.PendingUsers.Where(r => r.Email == request.Email));
    }

    [Fact]
    public async Task WhenUserSignsup_ThenSignupRequestIsSavedWithCorrectValues()
    {
        // When
        var FirstName = "Sarah";
        var LastName = "Ox";
        var Email = "Sarah@ox.com";
        var Password = "superSecure123";

        var request = new
        {
            FirstName,
            LastName,
            Email,
            Password
        };
        var response = await _client.PostAsJsonAsync(RouteConstants.SignupRoute, request);
        response.EnsureSuccessStatusCode();

        // Then
        var entiity = _dbContext.PendingUsers.FirstOrDefault(r => r.Email == request.Email);
        Assert.NotNull(entiity);
        Assert.Equal(FirstName, entiity.FirstName);
        Assert.Equal(LastName, entiity.LastName);
        Assert.Equal(Email, entiity.Email);
        Assert.Equal(Password, entiity.Password);

        var now = DateTime.UtcNow;
        Assert.InRange(entiity.CreatedAt, now.AddSeconds(-10), now.AddSeconds(10));
    }


}
