using BlobStorage.Repositories;
using BlobStorage.Service;

using CvPartner.Models;
using CvPartner.Repositories;

using Employees.Repositories;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Moq;
using Moq.AutoMock;

namespace IntegrationTests;

public class CustomWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup> where TStartup : class
{
    public readonly AutoMocker Mocker = new();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<EmployeeContext>));

            services.Remove(descriptor!);

            services.AddDbContextPool<EmployeeContext>(options =>
            {
                options.UseInMemoryDatabase(Guid.NewGuid().ToString());
            });

            var cvPartnerApiMock = Mocker.GetMock<ICvPartnerApiClient>();
            cvPartnerApiMock.Setup(client => client.GetAllEmployee(It.IsAny<string>())).ReturnsAsync(Array.Empty<CVPartnerUserDTO>());
            services.Replace(ServiceDescriptor.Transient(_ => cvPartnerApiMock.Object));
            
            // Creates mock of BlobStorageRepository and replaces it in program.cs. Returns string "test"
            var blobStorageServiceMock = Mocker.GetMock<IBlobStorageRepository>();
            blobStorageServiceMock.Setup(client => client.SaveToBlob(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("test");
            services.Replace(ServiceDescriptor.Transient(_ => blobStorageServiceMock.Object));
        });
    }
}