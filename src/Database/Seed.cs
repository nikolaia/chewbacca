namespace Database.Seed;

public static class Seed
{
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