using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kujanski.Mediator
{
    public interface IMediator
    {
        Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request);
        Task<List<TResponse>> SendNotificationAsync<TResponse>(IRequest<TResponse> request);
    }
}