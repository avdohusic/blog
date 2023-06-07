using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleBlog.Domain.Repositories;

public interface IBlogRepository
{
    Task AddBlog(Blog blog);

    Task<bool> DeleteBlog(Guid blogId);

    Task<IEnumerable<Blog>> GetAllBlogs();

    Task<Blog> GetBlogById(Guid blogId);

    Task<bool> UpdateBlog(Guid blogId, Blog updatedBlog);
}