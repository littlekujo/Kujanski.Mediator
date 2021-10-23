using System;
using System.Threading.Tasks;

namespace Kujanski.Mediator.Sample
{
    public class ThrowExceptionHandler : IHandler<GiveMeAValueRequest, string>
    {
        public Task<string> HandleAsync(GiveMeAValueRequest request)
        {
            throw new Exception("This is an exception from ThrowExceptionHandler");
            return Task.FromResult("Thrown");
        }
    }
}