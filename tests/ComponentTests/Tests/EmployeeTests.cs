using System.Net;
using System.Text;

using Employees.Models;
using Employees.Repositories;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

namespace IntegrationTests.Tests;

public class EmployeeTest :
    IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly EmployeeContext? _db;
    private readonly CustomWebApplicationFactory<Program> _factory;


    public EmployeeTest()
    {
        var factory = new CustomWebApplicationFactory<Program>();

        //using var scope = factory.Services.CreateScope();

        // var scopedServices = scope.ServiceProvider;
        // var db = scopedServices.GetRequiredService<EmployeeContext>();
        // db.Database.EnsureCreated();
        // Utilities.InitializeDbForTests(db);

        // var options = new DbContextOptionsBuilder<EmployeeContext>().UseInMemoryDatabase("TestDatabase").Options;
        // var db = new EmployeeContext(options);
        // Utilities.InitializeDbForTests(db);

        _client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        _factory = factory;
        // _db = db;
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
                config.Excluding(employee => employee.AccountNumber);
                config.Excluding(employee => employee.Address);
                config.Excluding(employee => employee.ZipCode);
                config.Excluding(employee => employee.City);

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
                config.Excluding(employee => employee.AccountNumber);
                config.Excluding(employee => employee.Address);
                config.Excluding(employee => employee.ZipCode);
                config.Excluding(employee => employee.City);

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
    public async void Given_EmployeeExists_When_CallingEmployeeControllerGETEmployeeExtended_Then_ReturnsSampleData()
    {
        // Arrange
        var knownSeedData = Seed.GetSeedingEmployees();
        var emergencyContactSeed = Seed.GetSeedingEmergencyContact(knownSeedData[0])[0];
        var allergiesAndDietaryPreferencesSeed = Seed.GetSeedingAllergiesAndDietaryPreferences(knownSeedData[0]);

        // Act
        var employeeResponse = await _client.GetAsync("/employees/test/extended?country=no");
        var content =
            JsonConvert.DeserializeObject<EmployeeExtendedJson>(await employeeResponse.Content
                .ReadAsStringAsync());

        // Assert
        content!.Email.Should().BeEquivalentTo(knownSeedData[0].Email);

        content!.EmergencyContact!.Should().BeEquivalentTo(emergencyContactSeed, config =>
            {
                config.Excluding(emergencyContact => emergencyContact.Id);
                config.Excluding(emergencyContact => emergencyContact.Employee);

                return config;
            });
        content!.AllergiesAndDietaryPreferences!.DefaultAllergies.Should().BeEquivalentTo(new List<string> { "MILK", "EGG" });
        content!.AllergiesAndDietaryPreferences!.OtherAllergies.Should().BeEquivalentTo(allergiesAndDietaryPreferencesSeed.OtherAllergies);
        content!.AllergiesAndDietaryPreferences!.DietaryPreferences.Should().BeEquivalentTo(new List<string> { "VEGETARIAN" });
        content!.AllergiesAndDietaryPreferences!.Comment.Should().BeEquivalentTo(allergiesAndDietaryPreferencesSeed.Comment);
    }

    [Fact]
    public async void Given_EmployeeDoesNotExists_When_CallingEmployeeControllerGETEmployee_Then_ReturnsNull()
    {
        // Act
        var employeeResponse = await _client.GetAsync("/employees/test");

        // Assert
        employeeResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async void Given_EmployeeExists_When_CallingEmployeeControllerPOSTAllergiesAndDietaryPreferences_Then_UpdateDatabase()
    {
        // Arrange
        var employee = Seed.GetSeedingEmployees()[0];
        var expectedAllergiesAndDietaryPreferences = new EmployeeAllergiesAndDietaryPreferencesEntity
        {
            Employee = employee,
            DefaultAllergies = new List<DefaultAllergyEnum> { DefaultAllergyEnum.MILK, DefaultAllergyEnum.GLUTEN, DefaultAllergyEnum.PEANUTS },
            OtherAllergies = new List<string> { "epler", "druer" },
            DietaryPreferences = new List<DietaryPreferenceEnum> { DietaryPreferenceEnum.NO_PREFERENCES },
            Comment = "Kjøtt må være helt gjennomstekt"
        };

        string json = @"{
                ""DefaultAllergies"": [""MILK"", ""GLUTEN"", ""PEANUTS""],
                ""OtherAllergies"": [""epler"", ""druer""],
                ""DietaryPreferences"": [""NO_PREFERENCES""],
                ""Comment"": ""Kjøtt må være helt gjennomstekt""
        }";

        var request = new HttpRequestMessage
        {
            RequestUri = new Uri("/employees/allergiesAndDietaryPreferences/no/test"),
            Method = HttpMethod.Post,
            Content = new StringContent(json, Encoding.UTF8, "application/json"),
        };

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<EmployeeContext>();
        db.Database.EnsureCreated();
        Utilities.InitializeDbForTests(db);

        // var dbEmployeeAllergiesAndDietaryPreferencesBefore = _db?.EmployeeAllergiesAndDietaryPreferences.FirstOrDefault(e => e.Employee.Email.Equals(employee.Email))!;
        // var dbEmployees = _db?.Employees;

        // Act
        var response = await _client.SendAsync(request);
        var responseBody = await response.Content.ReadAsStringAsync();
        // var content =
        //             JsonConvert.DeserializeObject<AllergiesAndDietaryPreferences>(await response.Content
        //                 .ReadAsStringAsync());

        // // Retrive from database
        var dbEmployeeAllergiesAndDietaryPreferences = db?.EmployeeAllergiesAndDietaryPreferences.FirstOrDefault(e => e.Employee.Email.Equals(employee.Email))!;
        var allAllergies = db?.EmployeeAllergiesAndDietaryPreferences;


        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        dbEmployeeAllergiesAndDietaryPreferences.Should().NotBeNull();
        dbEmployeeAllergiesAndDietaryPreferences!.Employee.Should().BeEquivalentTo(employee, config =>
            {
                config.Excluding(employee => employee.Id);

                return config;
            });
        dbEmployeeAllergiesAndDietaryPreferences!.DefaultAllergies.Should().BeEquivalentTo(expectedAllergiesAndDietaryPreferences.DefaultAllergies);
        dbEmployeeAllergiesAndDietaryPreferences!.OtherAllergies.Should().BeEquivalentTo(expectedAllergiesAndDietaryPreferences.OtherAllergies);
        dbEmployeeAllergiesAndDietaryPreferences!.DietaryPreferences.Should().BeEquivalentTo(expectedAllergiesAndDietaryPreferences.DietaryPreferences);
        dbEmployeeAllergiesAndDietaryPreferences!.Comment.Should().BeEquivalentTo(expectedAllergiesAndDietaryPreferences.Comment);
    }
}