using AutoFixture.Xunit2;

using Bemanning;

using CvPartner.Models;
using CvPartner.Repositories;

using Employees.Models;
using Employees.Repositories;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Moq;
using Moq.AutoMock;

using Xunit.Abstractions;

namespace IntegrationTests.Tests;

public class CvPartnerTest :
    IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly HttpClient _client;
    private readonly AutoMocker _mocker;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public CvPartnerTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _factory = new CustomWebApplicationFactory<Program>();
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions {AllowAutoRedirect = false});
        _mocker = _factory.Mocker;
    }

    // TODO: Add tests for bemanning date check. see if we can add a different date to an employee. Need to make sure CvPartnerDto and bemanning email are equal.

    [Theory, AutoData]
    public async void Given_CvPartnerEmployeesReturned_When_CallingCvPartnerControllerGET_Then_EnsureSavedToEmployeeDatabase(List<CVPartnerUserDTO> cvPartnerUserDtos,
        List<BemanningEmployee> bemanningEmployees)
    {
        // Arrange
        cvPartnerUserDtos.FirstOrDefault().email = bemanningEmployees.FirstOrDefault().Email;
        foreach (var VARIABLE in bemanningEmployees)
        {
            Console.WriteLine("LOOP B E: " + VARIABLE.Email);   
            Console.WriteLine("LOOP B S: " + VARIABLE.StartDate);   
        }
        var cvPartnerApiClientMock = _mocker.GetMock<ICvPartnerApiClient>();
        var returnCV  = cvPartnerApiClientMock.Setup(client => client.GetAllEmployee(It.IsAny<string>())).ReturnsAsync(cvPartnerUserDtos);

        var bemanningRepositoryMock = _mocker.GetMock<IBemanningRepository>();
        var returnBemanning = bemanningRepositoryMock.Setup(client => client.GetBemanningDataForEmployees())
            .ReturnsAsync(bemanningEmployees);

        // Act
        HttpResponseMessage schedulerResponse = await _client.GetAsync("/Scheduler");
        // Assert
        schedulerResponse.IsSuccessStatusCode.Should().BeTrue();
        
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<EmployeeContext>();
        db.Employees.Count().Should().Be(cvPartnerUserDtos.Count);

        Check_If_Employee_StartDate_Is_Updated(db.Employees);
        //Check_If_Employee_StartDate_Is_Updated(db.Employees).Should().BeTrue();
    }

    public Boolean Check_If_Employee_StartDate_Is_Updated(DbSet<EmployeeEntity> employees)
    {
        DateTime dateToCheck = new(year: 1, month: 1, day: 1);
        foreach (EmployeeEntity employee in employees)
        {
            if (employee.StartDate != dateToCheck)
            {
                Console.WriteLine("true");
            }

            Console.WriteLine(employee.StartDate);
            Console.WriteLine(employee.Email);
        }

        return false;
    }
}