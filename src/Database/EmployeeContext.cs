using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

using Database.Seed.Entities;

public class EmployeeContext : DbContext
{
    public EmployeeContext(DbContextOptions options) : base(options)
    {
        
    }
    
    public DbSet<Employee> Employees { get; set; }

    // // Example on how to seed data into the database:
    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<Blog>().HasData(new Blog { BlogId = 1, Url = "http://sample.com" }); 
    //
    //     base.OnModelCreating(modelBuilder);
    // }
}