using MediatR;

namespace CashlessRegistration.API.Domain.Queries.v1
{
    public abstract class AbstractQuery<T> : IRequest<T>
    {
    }
}