using MediatR;

namespace SimpleBlog.Application.Abstractions;

public interface IQuery : IRequest { }

public interface IQuery<out TResponse> : IRequest<TResponse> { }
