using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Kujanski.Mediator.Sample
{
    public class ChainThreeHandler: IHandler<ChainThreeRequest, bool>
    {
        public Task<bool> HandleAsync(ChainThreeRequest request)
        {
            Console.WriteLine("Hello from inside ChainThreeHandler.");
            return Task.FromResult(true);
        }
    }
}