using System.Net;

using AutoFixture.Xunit2;

using Bemanning;

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

    [Theory, AutoData]
    public async void
        Given_CvPartnerEmployeesReturned_When_CallingCvPartnerControllerGET_Then_EnsureSavedToEmployeeDatabase(
            List<CVPartnerUserDTO> cvPartnerUserDtos)
    {
        var bemanningEmployees =
            cvPartnerUserDtos.Select(dto => new BemanningEmployee(dto.email, DateTime.UtcNow.AddDays(-3), null)).ToList();
        
        // Arrange
        var cvPartnerApiClientMock = _mocker.GetMock<ICvPartnerApiClient>();
        cvPartnerApiClientMock.Setup(client => client.GetAllEmployee(It.IsAny<string>()))
            .ReturnsAsync(new ApiResponse<IEnumerable<CVPartnerUserDTO>>(new HttpResponseMessage(HttpStatusCode.OK),
                cvPartnerUserDtos, new RefitSettings()));

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
        db.Employees.Count().Should().Be(cvPartnerUserDtos.Count);
        
        //Check if updated date from Bemanning
        db.Employees.FirstOrDefault()!.StartDate.Should().NotBe(new DateTime(2018, 1, 1));

        // Check if blobService runs x amount of times
        var blobStorageServiceMocker = _mocker.GetMock<IBlobStorageRepository>();
        blobStorageServiceMocker.Verify(x => x.SaveToBlob(It.IsAny<string>(), It.IsAny<string>()),
            Times.Exactly(cvPartnerUserDtos.Count));
    }
}