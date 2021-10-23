using System;
using System.Threading.Tasks;

namespace Kujanski.Mediator.Sample
{
    public class PrintToConsoleHandler : IHandler<PrintToConsoleRequest, bool>
    {
        public Task<bool> HandleAsync(PrintToConsoleRequest request)
        {
            Console.WriteLine(request?.Text);
            return Task.FromResult(true);
        }
    }
}