using Microsoft.EntityFrameworkCore;

namespace SimpleBlog.Infrastructure.Repositories;

public class BlogRepository : IBlogRepository
{
    private readonly SimpleBlogDbContext dbContext;

    public BlogRepository(SimpleBlogDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IEnumerable<Blog>> GetAllBlogs()
    {
        return await dbContext.Blogs.ToListAsync();
    }

    public async Task<Blog> GetBlogById(int blogId)
    {
        return await dbContext.Blogs.FirstOrDefaultAsync(b => b.BlogId == blogId);
    }

    public async Task AddBlog(Blog blog)
    {
        dbContext.Blogs.Add(blog);
        await dbContext.SaveChangesAsync();
    }

    public async Task<bool> UpdateBlog(int blogId, Blog updatedBlog)
    {
        Blog existingBlog = await dbContext.Blogs.FirstOrDefaultAsync(b => b.BlogId == blogId);
        if (existingBlog != null)
        {
            existingBlog.Title = updatedBlog.Title;
            existingBlog.Content = updatedBlog.Content;
            existingBlog.Author = updatedBlog.Author;
            existingBlog.PublicationDate = updatedBlog.PublicationDate;
            await dbContext.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<bool> DeleteBlog(int blogId)
    {
        Blog blog = await dbContext.Blogs.FirstOrDefaultAsync(b => b.BlogId == blogId);
        if (blog != null)
        {
            dbContext.Blogs.Remove(blog);
            await dbContext.SaveChangesAsync();
            return true;
        }
        return false;
    }
}