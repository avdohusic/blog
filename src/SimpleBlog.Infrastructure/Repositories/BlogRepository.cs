using Microsoft.EntityFrameworkCore;
using SimpleBlog.Infrastructure.Data;

namespace SimpleBlog.Infrastructure.Repositories;

public class BlogRepository : IBlogRepository
{
    private readonly SimpleBlogDbContext _dbContext;

    public BlogRepository(SimpleBlogDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<IEnumerable<Blog>> GetAllBlogs()
    {
        return await _dbContext.Blogs.AsNoTracking().ToListAsync();
    }

    public async Task<Blog> GetBlogById(Guid blogId)
    {
        return await _dbContext.Blogs.AsNoTracking().FirstOrDefaultAsync(b => b.Id == blogId);
    }

    public async Task AddBlog(Blog blog)
    {
        _dbContext.Blogs.Add(blog);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> UpdateBlog(Guid blogId, Blog updatedBlog)
    {
        Blog existingBlog = await _dbContext.Blogs.FirstOrDefaultAsync(b => b.Id == blogId);
        if (existingBlog != null)
        {
            existingBlog
                .WithTitle(updatedBlog.Title)
                .WithContent(updatedBlog.Content)
                .WithPublicationDate(updatedBlog.PublicationDate);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<bool> DeleteBlog(Guid blogId)
    {
        Blog blog = await _dbContext.Blogs.FirstOrDefaultAsync(b => b.Id == blogId);
        if (blog != null)
        {
            _dbContext.Blogs.Remove(blog);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        return false;
    }
}