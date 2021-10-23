using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kujanski.Mediator.Sample
{
    public class LongDelayHandler : IHandler<LongDelayRequest, string>
    {
        public Task<string> HandleAsync(LongDelayRequest request)
        {
            Console.WriteLine("Start - Hello from inside LongDelayHandler.");
            Thread.Sleep(10000);
            Console.WriteLine("Done - Hello from inside LongDelayHandler.");
            return Task.FromResult("Hello from LongDelayHandler.");
        }
    }
}