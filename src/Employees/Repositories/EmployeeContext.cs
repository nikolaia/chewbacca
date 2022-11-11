using Employees.Models;

using Microsoft.EntityFrameworkCore;

namespace Employees.Repositories;

public class EmployeeContext : DbContext
{
    public EmployeeContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<EmployeeEntity> Employees { get; set; }

    // // Example on how to seed data into the database:
    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<Blog>().HasData(new Blog { BlogId = 1, Url = "http://sample.com" }); 
    //
    //     base.OnModelCreating(modelBuilder);
    // }
}