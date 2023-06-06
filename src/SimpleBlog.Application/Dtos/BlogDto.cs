namespace SimpleBlog.Application.Dtos;

public sealed record BlogDto
(
    int BlogId,
    string Title,
    string Content,
    string Author,
    DateTime PublicationDate
);
