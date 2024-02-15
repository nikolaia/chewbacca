using System.Net;
using System.Security.Claims;

using ApplicationCore.Interfaces;

using AutoFixture;

using FluentAssertions;

using Infrastructure;
using Infrastructure.ApiClients;
using Infrastructure.ApiClients.DTOs;
using Infrastructure.Entities;
using Infrastructure.Repositories;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

using Moq;
using Moq.AutoMock;

using Refit;

using Web;

namespace IntegrationTests.Tests;

public class OrchestratorTest :
    IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly AutoMocker _mocker;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public OrchestratorTest(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _mocker = _factory.Mocker;
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        var claims = new Dictionary<string, object> { { ClaimTypes.Name, "test@sample.com" } };
        _client.SetFakeBearerToken(claims);
    }

    [Fact]
    public async void
        Given_CvPartnerEmployeesReturned_When_CallingCvPartnerControllerGET_Then_EnsureSavedToEmployeeDatabase()
    {
        // Arrange
        var fixture = new Fixture();
        var cvPartnerUserDTOs = fixture.CreateMany<CVPartnerUserDTO>().ToList();

        var cvPartnerApiClientMock = _mocker.GetMock<ICvPartnerApiClient>();
        cvPartnerApiClientMock.Setup(client => client.GetAllEmployee(It.IsAny<string>()))
            .ReturnsAsync(new ApiResponse<IEnumerable<CVPartnerUserDTO>>(new HttpResponseMessage(HttpStatusCode.OK),
                cvPartnerUserDTOs, new RefitSettings()));

        var bemanningEmployees =
            cvPartnerUserDTOs.Select(dto => new VibesEmploymentDTO(dto.email, DateTime.UtcNow.AddDays(-3), null))
                .ToList();
        Mock<IVibesRepository> bemanningRepositoryMock = _mocker.GetMock<IVibesRepository>();
        bemanningRepositoryMock.Setup(client => client.GetEmployment())
            .ReturnsAsync(bemanningEmployees);

        // Act
        var employeeResponse = await _client.GetAsync("/Orchestrator");

        // Assert
        employeeResponse.IsSuccessStatusCode.Should().BeTrue();

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<EmployeeContext>();

        // Check if added
        db.Employees.Count().Should().Be(cvPartnerUserDTOs.Count);

        //Check if updated date from Bemanning
        db.Employees.FirstOrDefault()!.StartDate.Should().NotBe(new DateTime(2018, 1, 1));

        // Check if blobService runs x amount of times
        var blobStorageServiceMocker = _mocker.GetMock<IBlobStorageRepository>();
        blobStorageServiceMocker.Verify(x => x.SaveToBlob(It.IsAny<string>(), It.IsAny<string>()),
            Times.Exactly(cvPartnerUserDTOs.Count));
    }

    [Fact]
    public async void Given_CvPartnerCvReturned_When_CallingCvPartnerControllerGET_Then_EnsureSavedCvForEmployee()
    {
        // Arrange
        List<EmployeeEntity> employees = new()
        {
            new EmployeeEntity
            {
                Email = "test1@variant.no", CountryCode = "NO", Name = "Test", OfficeName = "Testgata"
            },
            new EmployeeEntity
            {
                Email = "test2@variant.no", CountryCode = "NO", Name = "Test", OfficeName = "Testgata"
            }
        };

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<EmployeeContext>();
        await db.AddRangeAsync(employees);
        await db.SaveChangesAsync();

        CVPartnerCvDTO cv =
            new()
            {
                email = "test1@variant.no",
                project_experiences = new List<Infrastructure.ApiClients.DTOs.ProjectExperience>
                {
                    new() { _id = "abc" , description = new Description(), long_description = new LongDescription()}
                }
            };

        List<CVPartnerUserDTO> cvUser = new()
        {
            new CVPartnerUserDTO() { email = "test1@variant.no", user_id = "test1"},
            new CVPartnerUserDTO() { email = "test2@variant.no" , user_id = "test2"}
        };


        var cvPartnerApiClientMock = _mocker.GetMock<ICvPartnerApiClient>();
        cvPartnerApiClientMock.Setup(client => client.GetAllEmployee(It.IsAny<string>()))
            .ReturnsAsync(new ApiResponse<IEnumerable<CVPartnerUserDTO>>(new HttpResponseMessage(HttpStatusCode.OK),
                cvUser, new RefitSettings()));
        

        cvPartnerApiClientMock.Setup(client =>
                client.GetEmployeeCv(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new ApiResponse<CVPartnerCvDTO>(new HttpResponseMessage(HttpStatusCode.OK),
                new CVPartnerCvDTO(), new RefitSettings()));
        
        cvPartnerApiClientMock.Setup(client =>
                client.GetEmployeeCv(It.Is
                    <string>(e => "test1".Equals(e)), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new ApiResponse<CVPartnerCvDTO>(new HttpResponseMessage(HttpStatusCode.OK),
                cv, new RefitSettings()));

        var bemanningEmployees =
            cvUser.Select(dto => new VibesEmploymentDTO(dto.email, DateTime.UtcNow.AddDays(-3), null))
                .ToList();
        Mock<IVibesRepository> bemanningRepositoryMock = _mocker.GetMock<IVibesRepository>();
        bemanningRepositoryMock.Setup(client => client.GetEmployment())
            .ReturnsAsync(bemanningEmployees);

        // Act
        var employeeResponse = await _client.GetAsync("/Orchestrator/cv");

        // Assert
        employeeResponse.IsSuccessStatusCode.Should().BeTrue();


        db.ProjectExperiences.Count().Should().BeGreaterThan(0);
    }
}