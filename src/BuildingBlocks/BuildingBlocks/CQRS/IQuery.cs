using MediatR;

namespace BuildingBlocks.CQRS;

interface IQuery<out TResponse> : IRequest<TResponse>
    where TResponse : notnull
{

}