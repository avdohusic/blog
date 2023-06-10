using Microsoft.EntityFrameworkCore;

namespace SimpleBlog.Infrastructure.Data.Seed;

public static class ModelBuilderExtensions
{
    public static void SeedUsers(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserIdentity>().HasData(
            new UserIdentity
            {
                Id = 1,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                PasswordHash = "AQAAAAIAAYagAAAAEHrrqTffzR2ZG8OWXWR7K4tRYX3R8vYaiOdqLfWzVBqdIChSLNJfKe1J2tuAWQN1rA==", // Test123!
                SecurityStamp = "CIMPVS4PPET7EIUGOTDT3WJYWM6JODXB",
                Email = "admin@mail.com",
                NormalizedEmail = "ADMIN@MAIL.COM"
            },
            new UserIdentity
            {
                Id = 2,
                UserName = "publisher",
                NormalizedUserName = "PUBLISHER",
                PasswordHash = "AQAAAAIAAYagAAAAEHrrqTffzR2ZG8OWXWR7K4tRYX3R8vYaiOdqLfWzVBqdIChSLNJfKe1J2tuAWQN1rA==", // Test123!
                SecurityStamp = "CIMPVS4PPET7EIUGOTDT3WJYWM6JODXB",
                Email = "publisher@mail.com",
                NormalizedEmail = "PUBLISHER@MAIL.COM"
            },
            new UserIdentity
            {
                Id = 3,
                UserName = "user",
                NormalizedUserName = "USER",
                PasswordHash = "AQAAAAIAAYagAAAAEHrrqTffzR2ZG8OWXWR7K4tRYX3R8vYaiOdqLfWzVBqdIChSLNJfKe1J2tuAWQN1rA==", // Test123!
                SecurityStamp = "CIMPVS4PPET7EIUGOTDT3WJYWM6JODXB",
                Email = "user@mail.com",
                NormalizedEmail = "USER@MAIL.COM"
            }
        );
    }

    public static void SeedIdentityRoles(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserIdentityRole>().HasData(
            new UserIdentityRole
            {
                Id = 1,
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR",
            },
            new UserIdentityRole
            {
                Id = 2,
                Name = "Publisher",
                NormalizedName = "PUBLISHER",
            },
            new UserIdentityRole
            {
                Id = 3,
                Name = "User",
                NormalizedName = "USER",
            }
       );
    }

    public static void SeedUsersRoles(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserIdentityUserRole>().HasData(
            new UserIdentityUserRole { RoleId = 1, UserId = 1 },
            new UserIdentityUserRole { RoleId = 2, UserId = 2 },
            new UserIdentityUserRole { RoleId = 3, UserId = 3 }
       );
    }
}