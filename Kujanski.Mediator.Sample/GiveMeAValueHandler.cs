using System;
using System.Threading.Tasks;

namespace Kujanski.Mediator.Sample
{
    public class GiveMeAValueHandler : IHandler<GiveMeAValueRequest, string>
    {
        public Task<string> HandleAsync(GiveMeAValueRequest request)
        {
            Console.WriteLine("Hello from inside GiveMeAValue.");
            return Task.FromResult("Hello from GiveMeAValue.");
        }
    }
}