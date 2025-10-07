using MediatR;

namespace Template.Application.Queries;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}
