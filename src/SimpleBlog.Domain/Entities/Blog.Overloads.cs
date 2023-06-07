using System;

namespace SimpleBlog.Domain.Entities;
public partial class Blog
{
    public static Blog New(string title, string content)
    {
        return new Blog { Id = Guid.NewGuid(), Title = title, Content = content };
    }

    public Blog WithAuthor(string author)
    {
        Author = author;
        return this;
    }

    public Blog WithContent(string content)
    {
        Content = content;
        return this;
    }

    public Blog WithTitle(string title)
    {
        Title = title;
        return this;
    }

    public Blog WithPublicationDate(DateTime publicationDate)
    {
        PublicationDate = publicationDate;
        return this;
    }

}
