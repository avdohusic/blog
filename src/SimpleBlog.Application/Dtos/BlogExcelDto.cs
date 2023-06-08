using System.ComponentModel;

namespace SimpleBlog.Application.Dtos;
public class BlogExcelDto
{
    [DisplayName("Blog ID")]
    public Guid BlogId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Author { get; set; }
    [DisplayName("Publication Date")]
    public DateTime PublicationDate { get; set; }
}
