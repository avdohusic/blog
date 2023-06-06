using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SimpleBlog.Infrastructure.Configurations;

public class BlogConfiguration : IEntityTypeConfiguration<Blog>
{
    public void Configure(EntityTypeBuilder<Blog> builder)
    {
        builder.ToTable("Blog");

        builder.HasKey(x => x.BlogId);

        builder.Property(e => e.Title)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(e => e.Content)
               .IsRequired();

        builder.Property(e => e.Author)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(e => e.PublicationDate)
               .IsRequired();
    }
}