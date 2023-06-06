using MediatR;

namespace SimpleBlog.Application.Abstractions;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
{ }

public interface IQueryHandler<in TQuery> : IRequestHandler<TQuery>
        where TQuery : IQuery
{ }