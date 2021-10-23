using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Kujanski.Mediator
{
    public class Mediator : IMediator
    {
        private readonly Func<Type, object> _serviceResolver;
        private readonly IDictionary<Type, List<Type>> _handlerDetails;
        private ConcurrentDictionary<string, MethodInfo> _methodInfoCache;

        public Mediator(Func<Type, object> serviceResolver, IDictionary<Type, List<Type>> handlerDetails)
        {
            _serviceResolver = serviceResolver;
            _handlerDetails = handlerDetails;
            _methodInfoCache = new ConcurrentDictionary<string, MethodInfo>();
        }
        
        public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            var requestHandlerType = GetTypes(request);
            
            var handler = _serviceResolver(requestHandlerType?.FirstOrDefault());
            if (handler == null)
                throw new Exception($"No handler to handle request of type {request.GetType().Name}.");
            
            return await Execute(handler, request);
        }
        
        public async Task<List<TResponse>> SendNotificationAsync<TResponse>(IRequest<TResponse> request)
        {
            var requestHandlerType = GetTypes(request);
            
            List<Task<TResponse>> tasks = new List<Task<TResponse>>();
            List<Exception> exceptions = new List<Exception>();
            List<TResponse> responses = new List<TResponse>();

            foreach (var cur in requestHandlerType)
            {
                var handler = _serviceResolver(cur);
                if (handler == null)
                    continue;

                var task = Task.Run(() => Execute(handler, request));
                tasks.Add(task);
            }

            var taskAggregate = Task.WhenAll(tasks);
            try
            {
                await taskAggregate;
            }
            catch (Exception ex)
            {
                if (taskAggregate.Exception != null)
                {
                    var faulted = tasks.Where(t => t.IsFaulted).ToList();
                    foreach (var curFaulted in faulted)
                    {
                        exceptions.Add(curFaulted.Exception?.InnerException);
                    }
                }
            }

            var completed = tasks.Where(t => t.IsCompletedSuccessfully).ToList();
            foreach (var curCompleted in completed)
            {
                responses.Add(curCompleted.Result);
            }
            
            return responses;
        }

        private async Task<TResponse> Execute<TResponse>(object handler, IRequest<TResponse> request)
        {
            try
            {
                var typeName = handler.GetType().FullName;
                MethodInfo methodInfo = null;
                bool foundInCache = false;
                
                if (_methodInfoCache.ContainsKey(typeName))
                    foundInCache = _methodInfoCache.TryGetValue(typeName, out methodInfo);
                
                if (foundInCache && methodInfo != null)
                    return await (Task<TResponse>) methodInfo.Invoke(handler, new[] {request});

                methodInfo = handler.GetType().GetMethod("HandleAsync");
                _methodInfoCache.TryAdd(typeName, methodInfo);
                return await (Task<TResponse>) methodInfo.Invoke(handler, new[] {request});
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        private List<Type> GetTypes<TResponse>(IRequest<TResponse> request)
        {
            var requestType = request.GetType();
            if (!_handlerDetails.ContainsKey(requestType))
                throw new Exception($"No handler to handle request of type {requestType.Name}.");

            _handlerDetails.TryGetValue(requestType, out var requestHandlerType);
            return requestHandlerType;
        }
    }

}