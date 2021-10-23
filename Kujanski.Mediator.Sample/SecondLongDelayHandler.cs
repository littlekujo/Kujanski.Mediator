using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kujanski.Mediator.Sample
{
    public class SecondLongDelayHandler : IHandler<LongDelayRequest, string>
    {
        public Task<string> HandleAsync(LongDelayRequest request)
        {
            Console.WriteLine("Start - Hello from inside SecondLongDelayHandler.");
            Thread.Sleep(5000);
            Console.WriteLine("Done - Hello from inside SecondLongDelayHandler.");
            return Task.FromResult("Hello from SecondLongDelayHandler.");
        }
    }
}