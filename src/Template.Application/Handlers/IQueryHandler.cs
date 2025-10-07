using MediatR;
using Template.Application.Queries;

namespace Template.Application.Handlers;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
}
