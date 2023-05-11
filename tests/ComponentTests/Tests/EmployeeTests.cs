using Employees.Models;
using Employees.Repositories;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

namespace IntegrationTests.Tests;

public class EmployeeTest :
    IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public EmployeeTest()
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
    public async void Given_EmployeeExists_When_CallingEmployeeControllerGET_Then_ReturnsSampleData()
    {
        // Arrange
        var knownSeedData = Seed.GetSeedingEmployees();

        // Act
        var employeeResponse = await _client.GetAsync("/employees");
        var content =
            JsonConvert.DeserializeObject<EmployeesJson>(await employeeResponse.Content
                .ReadAsStringAsync());

        // Assert
        content!.Employees.Should().BeEquivalentTo(knownSeedData, config =>
            {
                config.Excluding(employee => employee.Id);
                config.Excluding(employee => employee.EndDate);
                config.Excluding(employee => employee.CountryCode);
                return config;
            }
        );
    }

    [Fact]
    public async void Given_EmployeeExists_When_CallingEmployeeControllerGETByCountry_Then_ReturnsSampleData()
    {
        // Arrange
        var knownSeedData = Seed.GetSeedingEmployees();

        // Act
        var employeeResponse = await _client.GetAsync("/employees?country=no");
        var content =
            JsonConvert.DeserializeObject<EmployeesJson>(await employeeResponse.Content
                .ReadAsStringAsync());

        // Assert
        content!.Employees.Should().BeEquivalentTo(knownSeedData.Where(emp => emp.CountryCode == "no"), config =>
            {
                config.Excluding(employee => employee.Id);
                config.Excluding(employee => employee.EndDate);
                config.Excluding(employee => employee.CountryCode);
                return config;
            }
        );
    }

    [Fact]
    public async void Given_EmployeeExists_When_CallingEmployeeControllerGETEmployee_Then_ReturnsSampleData()
    {
        // Arrange
        var knownSeedData = Seed.GetSeedingEmployees();

        // Act
        var employeeResponse = await _client.GetAsync("/employees/test?country=no");
        var content =
            JsonConvert.DeserializeObject<EmployeeJson>(await employeeResponse.Content
                .ReadAsStringAsync());

        // Assert
        content!.Email.Should().BeEquivalentTo(knownSeedData[0].Email);
    }

    [Fact]
    public async void Given_EmployeeDoesNotExists_When_CallingEmployeeControllerGETEmployee_Then_ReturnsNull()
    {
        // Act
        var employeeResponse = await _client.GetAsync("/employees/test");

        // Assert
        employeeResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }
}