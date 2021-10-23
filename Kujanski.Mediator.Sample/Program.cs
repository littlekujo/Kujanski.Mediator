using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kujanski.Mediator.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Kujanski.Mediator.Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                //.AddTransient<PrintToConsoleHandler>()
                .AddMediator(ServiceLifetime.Scoped, typeof(Program))
                .BuildServiceProvider();

            // var handlerDetails = new Dictionary<Type, Type>()
            // {
            //     {typeof(PrintToConsoleRequest), typeof(PrintToConsoleHandler)}
            // };

            var request = new PrintToConsoleRequest()
            {
                Text = "Hello from mediator"
            };

            var mediator = serviceProvider.GetRequiredService<IMediator>();
            await mediator.SendAsync(request);
            var result = await mediator.SendAsync(new GiveMeAValueRequest());
            Console.WriteLine(result);
            await mediator.SendNotificationAsync(new GiveMeAValueRequest());

            try
            {
                await mediator.SendAsync(new NoHandlerRequest());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            try
            {
                await mediator.SendNotificationAsync(new NoHandlerRequest());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            Console.WriteLine("Starting long delay notification.");
            await mediator.SendNotificationAsync(new LongDelayRequest());
            Console.WriteLine("Done with long delay notification.");
            
            var chainResult = await mediator.SendAsync(new ChainOneRequest());
            Console.WriteLine($"Done with chain result = {chainResult}.");
        }
    }
}
