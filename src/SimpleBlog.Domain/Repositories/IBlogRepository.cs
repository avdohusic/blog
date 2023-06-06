namespace SimpleBlog.Domain.Repositories;

public interface IBlogRepository
{
    Task AddBlog(Blog blog);
    Task<bool> DeleteBlog(int blogId);
    Task<IEnumerable<Blog>> GetAllBlogs();
    Task<Blog> GetBlogById(int blogId);
    Task<bool> UpdateBlog(int blogId, Blog updatedBlog);
}