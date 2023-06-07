using SimpleBlog.Domain.Entities.Abstract;
using System;

namespace SimpleBlog.Domain.Entities;

public sealed partial class Blog : BaseEntity<Guid>
{
    public string Title { get; set; }

    public string Content { get; set; }

    public string Author { get; set; }

    public DateTime PublicationDate { get; set; }
}