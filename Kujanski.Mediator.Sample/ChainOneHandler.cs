using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Kujanski.Mediator.Sample
{
    public class ChainOneHandler: IHandler<ChainOneRequest, bool>
    {
        private readonly IServiceProvider _serviceProvider;
        public ChainOneHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        public async Task<bool> HandleAsync(ChainOneRequest request)
        {
            Console.WriteLine("Hello from inside ChainOneHandler.");
            var mediator = _serviceProvider.GetRequiredService<IMediator>();
            return await mediator.SendAsync(new ChainTwoRequest());
        }
    }
}