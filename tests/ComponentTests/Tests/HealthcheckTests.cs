using FluentAssertions;

using Infrastructure;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

using Shared;

using Web;

namespace IntegrationTests.Tests;

public class HealthcheckTest :
    IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public HealthcheckTest()
    {
        var factory = new CustomWebApplicationFactory<Program>();

        using var scope = factory.Services.CreateScope();

        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<EmployeeContext>();
        db.Database.EnsureCreated();
        Utilities.InitializeDbForTests(db);

        _client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
    }

    [Fact]
    public async void HealthcheckEndpoint_Should_Return()
    {
        // Act
        var healthcheckResponse = await _client.GetAsync("/healthcheck");
        var content = JsonConvert.DeserializeObject<HealthcheckResponse>(await healthcheckResponse.Content
            .ReadAsStringAsync());
        
        // Assert
        content.Should().NotBeNull();
        content?.Database.Should().Be(true);
        content?.AppConfig.Should().Be(false);
        content?.KeyVault.Should().Be(false);
    }
}