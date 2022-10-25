using Database.Seed;

using Employee.Repositories;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc.Testing;

using Newtonsoft.Json;

namespace IntegrationTests.BloggingContextTests;

public class EmployeeTest :
    IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public EmployeeTest(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions {AllowAutoRedirect = false});
    }

    [Fact]
    public async void Given_BlogExists_When_CallingEmployeeControllerGET_Then_ReturnsSampleData()
    {
        // Arrange
        var knownSeedData = Seed.GetSeedingEmployees();

        // Act
        var blogsResponse = await _client.GetAsync("/Employee");
        var content = JsonConvert.DeserializeObject<List<Employee>>(await blogsResponse.Content.ReadAsStringAsync());

        // Assert
        content.Should().BeEquivalentTo(knownSeedData, options =>
            options.Excluding(employee => employee.Id)
        );
    }
}