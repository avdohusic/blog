using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleBlog.Domain.Repositories;

public interface IBlogRepository
{
    Task AddBlogAsync(Blog blog);

    Task<bool> DeleteBlogAsync(Guid blogId);

    Task<IEnumerable<Blog>> GetAllBlogsAsync();

    Task<Blog> GetBlogByIdAsync(Guid blogId);

    Task<bool> UpdateBlogAsync(Guid blogId, Blog updatedBlog);
}