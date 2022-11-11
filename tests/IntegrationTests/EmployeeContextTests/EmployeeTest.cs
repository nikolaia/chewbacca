using Employees;
using FluentAssertions;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

using Newtonsoft.Json;

namespace IntegrationTests.EmployeeContextTests;

public class EmployeeTest :
    IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public EmployeeTest(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions {AllowAutoRedirect = false});
    }

    [Fact]
    public async void Given_EmployeeExists_When_CallingEmployeeControllerGET_Then_ReturnsSampleData()
    {
        // Arrange
        var knownSeedData = Seed.GetSeedingEmployees();

        // Act
        var employeeResponse = await _client.GetAsync("/Employee");
        var content = JsonConvert.DeserializeObject<List<Employees.Models.Employee>>(await employeeResponse.Content.ReadAsStringAsync());

        // Assert
        content.Should().BeEquivalentTo(knownSeedData, options =>
            options.Excluding(employee => employee.Id)
        );
    }
}