using System;
using System.Threading.Tasks;

namespace Kujanski.Mediator.Sample
{
    public class SecondGiveMeAValueHandler: IHandler<GiveMeAValueRequest, string>
    {
        public Task<string> HandleAsync(GiveMeAValueRequest request)
        {
            Console.WriteLine("Hello from inside SecondGiveMeAValue.");
            return Task.FromResult("Hello from SecondGiveMeAValue.");
        }
    }
}