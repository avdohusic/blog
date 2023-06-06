using Microsoft.EntityFrameworkCore;

namespace SimpleBlog.Infrastructure.Extensions;

public static class ModelBuilderExtensions
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserIdentity>().HasData(
            new UserIdentity
            {
                Id = 1,
                UserName = "avdohusic",
                NormalizedUserName = "AVDOHUSIC",
                PasswordHash = "AQAAAAIAAYagAAAAEHrrqTffzR2ZG8OWXWR7K4tRYX3R8vYaiOdqLfWzVBqdIChSLNJfKe1J2tuAWQN1rA==", // Test123!
                SecurityStamp = "CIMPVS4PPET7EIUGOTDT3WJYWM6JODXB",
                Email = "avdo.husic@gmail.com",
                NormalizedEmail = "AVDO.HUSIC@GMAIL.COM"
            }
        );
    }
}
