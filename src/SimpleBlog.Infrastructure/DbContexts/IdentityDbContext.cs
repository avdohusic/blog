using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimpleBlog.Domain.Constants;

namespace SimpleBlog.Infrastructure.DbContexts;

public class IdentityDbContext : IdentityDbContext<UserIdentity, UserIdentityRole, int, UserIdentityUserClaim, UserIdentityUserRole, UserIdentityUserLogin, UserIdentityRoleClaim, UserIdentityUserToken>
{
    public IdentityDbContext(DbContextOptions<SimpleBlogDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        ConfigureIdentityContext(builder);
    }

    private void ConfigureIdentityContext(ModelBuilder builder)
    {
        builder.Entity<UserIdentityRole>().ToTable(IdentityTableConsts.IdentityRoles);
        builder.Entity<UserIdentityRoleClaim>().ToTable(IdentityTableConsts.IdentityRoleClaims);
        builder.Entity<UserIdentityUserRole>().ToTable(IdentityTableConsts.IdentityUserRoles);

        builder.Entity<UserIdentity>().ToTable(IdentityTableConsts.IdentityUsers);
        builder.Entity<UserIdentityUserLogin>().ToTable(IdentityTableConsts.IdentityUserLogins);
        builder.Entity<UserIdentityUserClaim>().ToTable(IdentityTableConsts.IdentityUserClaims);
        builder.Entity<UserIdentityUserToken>().ToTable(IdentityTableConsts.IdentityUserTokens);
    }
}