using System.Reflection;

namespace MinimalApiTemplate.Routing
{
    //Thanks to https://github.com/marcominerva/MinimalHelpers.Routing
    //We can even use the NuGet Package
    public static class IEndpointRouteBuilderExtensions
    {
        public static void MapEndpoints(this IEndpointRouteBuilder endpoints, Func<Type, bool>? predicate = null)
            => MapEndpoints(endpoints, Assembly.GetCallingAssembly(), predicate);

        public static void MapEndpoints(this IEndpointRouteBuilder endpoints, Assembly assembly, Func<Type, bool>? predicate = null)
        {
            ArgumentNullException.ThrowIfNull(endpoints);
            ArgumentNullException.ThrowIfNull(assembly);

            var endpointRouteHandlerInterfaceType = typeof(IEndpointRouteHandler);

            var endpointRouteHandlerTypes = assembly.GetTypes().Where(t =>
                t.IsClass && !t.IsAbstract && !t.IsGenericType
                && t.GetConstructor(Type.EmptyTypes) != null
                && endpointRouteHandlerInterfaceType.IsAssignableFrom(t)
                && (predicate?.Invoke(t) ?? true));

            foreach (var endpointRouteHandlerType in endpointRouteHandlerTypes)
            {
                var handler = (IEndpointRouteHandler)Activator.CreateInstance(endpointRouteHandlerType)!;
                handler.MapEndpoints(endpoints);
            }
        }

        public static void MapEndpointsFromAssemblyContaining<T>(this IEndpointRouteBuilder endpoints, Func<Type, bool>? predicate = null) where T : class
            => MapEndpoints(endpoints, typeof(T).Assembly, predicate);
    }
}
