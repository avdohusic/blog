using Microsoft.EntityFrameworkCore;
using SimpleBlog.Infrastructure.Data;

namespace SimpleBlog.Infrastructure.Repositories;

public class BlogRepository : IBlogRepository
{
    private readonly SimpleBlogDbContext _dbContext;

    public BlogRepository(SimpleBlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Blog>> GetAllBlogsAsync()
    {
        return await _dbContext.Blogs.AsNoTracking().ToListAsync();
    }

    public async Task<Blog> GetBlogByIdAsync(Guid blogId)
    {
        return await _dbContext.Blogs.AsNoTracking().FirstOrDefaultAsync(b => b.Id == blogId);
    }

    public async Task AddBlogAsync(Blog blog)
    {
        _dbContext.Blogs.Add(blog);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> UpdateBlogAsync(Guid blogId, Blog updatedBlog)
    {
        var existingBlog = await _dbContext.Blogs.FirstOrDefaultAsync(b => b.Id == blogId);
        if (existingBlog != null)
        {
            existingBlog
                .WithTitle(updatedBlog.Title)
                .WithContent(updatedBlog.Content)
                .WithAuthor(updatedBlog.Author)
                .WithPublicationDate(updatedBlog.PublicationDate);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<bool> DeleteBlogAsync(Guid blogId)
    {
        var blog = await _dbContext.Blogs.FirstOrDefaultAsync(b => b.Id == blogId);
        if (blog != null)
        {
            _dbContext.Blogs.Remove(blog);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        return false;
    }
}