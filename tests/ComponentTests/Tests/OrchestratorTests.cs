using System.Net;

using AutoFixture;

using Bemanning.Repositories;

using BlobStorage.Repositories;

using CvPartner.Models;
using CvPartner.Repositories;

using Employees.Repositories;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

using Moq;
using Moq.AutoMock;

using Refit;

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
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        _mocker = _factory.Mocker;
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
            cvPartnerUserDTOs.Select(dto => new BemanningEmployee(dto.email, DateTime.UtcNow.AddDays(-3), null))
                .ToList();
        Mock<IBemanningRepository> bemanningRepositoryMock = _mocker.GetMock<IBemanningRepository>();
        bemanningRepositoryMock.Setup(client => client.GetBemanningDataForEmployees())
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
}