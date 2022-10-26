using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using Shared;

namespace Employee.Repositories;

public class EmployeeContext : DbContext
{
    private readonly AzureServiceTokenProvider _azureServiceTokenProvider;

    public EmployeeContext(DbContextOptions options, AzureServiceTokenProvider azureServiceTokenProvider) : base(options)
    {
        this._azureServiceTokenProvider = azureServiceTokenProvider;
    }

    public DbSet<Models.Employee> Employees { get; set; }

    // // Example on how to seed data into the database:
    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<Blog>().HasData(new Blog { BlogId = 1, Url = "http://sample.com" }); 
    //
    //     base.OnModelCreating(modelBuilder);
    // }
}