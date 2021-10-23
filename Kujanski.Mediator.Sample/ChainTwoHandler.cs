using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Kujanski.Mediator.Sample
{
    public class ChainTwoHandler: IHandler<ChainTwoRequest, bool>
    {
        private readonly IServiceProvider _serviceProvider;
        public ChainTwoHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        public async Task<bool> HandleAsync(ChainTwoRequest request)
        {
            Console.WriteLine("Hello from inside ChainTwoHandler.");
            var mediator = _serviceProvider.GetRequiredService<IMediator>();
            return await mediator.SendAsync(new ChainThreeRequest());
        }
    }
}