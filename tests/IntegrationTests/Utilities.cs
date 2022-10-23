namespace IntegrationTests;

public static class Utilities
{
    public static void InitializeDbForTests(BloggingContext db)
    {
        db.Blogs.AddRange(GetSeedingBlogs());
        db.Posts.AddRange(GetSeedingPosts());
        db.SaveChanges();
    }

    public static void ReinitializeDbForTests(BloggingContext db)
    {
        db.Blogs.RemoveRange(db.Blogs);
        db.Posts.RemoveRange(db.Posts);
        InitializeDbForTests(db);
    }

    public static List<Blog> GetSeedingBlogs()
    {
        return new List<Blog>()
        {
            new Blog { BlogId = 1, Url = "http://sample.com" }
        };
    }
    
    public static List<Post> GetSeedingPosts()
    {
        return new List<Post>()
        {
            new Post { BlogId = 1, PostId = 1, Title = "First post", Content = "Test 1" }
        };
    }
}