using SimpleBlog.Application.Common.Exceptions;
using SimpleBlog.Domain.Constants;
using SimpleBlog.Domain.Shared;
using System.Net;

namespace SimpleBlog.Api.Helpers;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            var validationResult = new ValidationResult()
            {
                Title = MessageConstants.ValidationTitle,
                Status = context.Response.StatusCode,
                Message = MessageConstants.ValidationMessage,
                Errors = exception.Errors
            };
            await context.Response.WriteAsJsonAsync(validationResult);
        }
        catch (ResourceProhibitedException exception)
        {
            _logger.LogWarning(exception, exception.Message);

            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            var errorResult = new ErrorResult()
            {
                Title = MessageConstants.BadRequestTitle,
                Status = context.Response.StatusCode,
                Message = exception.Message,
            };
            await context.Response.WriteAsJsonAsync(errorResult);
        }
        catch (NotFoundException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            var errorResult = new ErrorResult()
            {
                Title = MessageConstants.NotFoundTitle,
                Status = context.Response.StatusCode,
                Message = MessageConstants.NotFoundMessage,
            };
            await context.Response.WriteAsJsonAsync(errorResult);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var errorResult = new ErrorResult()
            {
                Title = MessageConstants.ServerErrorTitle,
                Status = context.Response.StatusCode,
                Message = MessageConstants.ServerErrorMessage,
            };
            await context.Response.WriteAsJsonAsync(errorResult);
        }
    }
}
