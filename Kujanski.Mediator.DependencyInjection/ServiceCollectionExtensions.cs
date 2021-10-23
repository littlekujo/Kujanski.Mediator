using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kujanski.Mediator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Kujanski.Mediator.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMediator(this IServiceCollection services,
            ServiceLifetime serviceLifetime,
            params Type[] markers)
        {
            var handlerInfo = new Dictionary<Type, List<Type>>();
            foreach (var marker in markers)
            {
                var assembly = marker.Assembly;
                var requests = GetClassesImplementingInterface(assembly, typeof(IRequest<>));
                var handlers = GetClassesImplementingInterface(assembly, typeof(IHandler<,>));

                // foreach (var request in requests)
                // {
                //     var handlerFound = 
                //         handlers.FirstOrDefault(xx => request == xx.GetInterface("IHandler`2")!.GetGenericArguments()[0]);
                //     handlerInfo[request] = handlerFound;
                // }
                
                foreach (var request in requests)
                {
                    var handlerFound = 
                        handlers.Where(xx => request == xx.GetInterface("IHandler`2")!.GetGenericArguments()[0]);
                    foreach (var cur in handlerFound)
                    {
                        if (!handlerInfo.ContainsKey(request))
                            handlerInfo[request] = new List<Type>();
                        handlerInfo[request].Add(cur);
                    }
                }

                var serviceDescriptor = handlers.Select(x => new ServiceDescriptor(x, x, serviceLifetime));
                services.TryAdd(serviceDescriptor);
            }

            services.AddSingleton<IMediator>(x => new Kujanski.Mediator.Mediator(x.GetRequiredService, handlerInfo));
            
            return services;
        }

        private static List<Type> GetClassesImplementingInterface(Assembly assembly, Type typeToMatch)
        {
            return assembly.ExportedTypes
                .Where(type =>
                {
                    var genericInterfaceTypes = type.GetInterfaces().Where(x => x.IsGenericType);
                    var implementRequestType = genericInterfaceTypes
                        .Any(x => x.GetGenericTypeDefinition() == typeToMatch);

                    return !type.IsInterface && !type.IsAbstract && implementRequestType;
                }).ToList();
        }
    }
}