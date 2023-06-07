namespace SimpleBlog.Application.Dtos;

public sealed record BlogDto
(
    Guid BlogId,
    string Title,
    string Content,
    string Author,
    DateTime PublicationDate
);