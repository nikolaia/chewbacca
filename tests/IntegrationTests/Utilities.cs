using Database.Seed;

namespace IntegrationTests;

public static class Utilities
{
    public static void InitializeDbForTests(BloggingContext db)
    {
        db.Blogs.AddRange(Seed.GetSeedingBlogs());
        db.Posts.AddRange(Seed.GetSeedingPosts());
        db.SaveChanges();
    }

    public static void ReinitializeDbForTests(BloggingContext db)
    {
        db.Blogs.RemoveRange(db.Blogs);
        db.Posts.RemoveRange(db.Posts);
        InitializeDbForTests(db);
    }


    

}