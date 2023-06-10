using Microsoft.EntityFrameworkCore;
using SimpleBlog.Infrastructure.Data.Seed;

namespace SimpleBlog.Infrastructure.Data;

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

        modelBuilder.SeedUsers();
        modelBuilder.SeedIdentityRoles();
        modelBuilder.SeedUsersRoles();

        base.OnModelCreating(modelBuilder);
    }
}