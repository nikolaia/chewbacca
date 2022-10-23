using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

public class BloggingContext : DbContext
{
    public BloggingContext(DbContextOptions options) : base(options)
    {
        
    }
    
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>().HasData(new Blog { BlogId = 1, Url = "http://sample.com" }); 
        
        modelBuilder.Entity<Post>().HasData(
            new Post { BlogId = 1, PostId = 1, Title = "First post", Content = "Test 1" }); 

        base.OnModelCreating(modelBuilder);
    }
}

public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }

    public List<Post> Posts { get; } = new();
}

public class Post
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }

    public int BlogId { get; set; }
    public Blog Blog { get; set; }
} 
