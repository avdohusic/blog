using Microsoft.EntityFrameworkCore;
using SimpleBlog.Infrastructure.Extensions;

namespace SimpleBlog.Infrastructure.DbContexts;

public class SimpleBlogDbContext : IdentityDbContext
{
    public SimpleBlogDbContext(DbContextOptions<SimpleBlogDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Blog> Blogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InfrastructureAssembly).Assembly);
        modelBuilder.Seed();

        base.OnModelCreating(modelBuilder);
    }
}