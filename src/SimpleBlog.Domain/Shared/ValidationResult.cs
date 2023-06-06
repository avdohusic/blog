namespace SimpleBlog.Domain.Shared;

public class ValidationResult : ErrorResult
{
    public IDictionary<string, string[]> Errors { get; set; }
}