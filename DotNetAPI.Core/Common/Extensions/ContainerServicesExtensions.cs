using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DotNetAPI.Core.Common.Extensions;

public static class ContainerServicesExtensions
{
    private static ServiceDescriptor RefactorDecorateeDescriptor(ServiceDescriptor decorateeDescriptor)
    {
        Type decorateeImplType = decorateeDescriptor.GetImplementationType();

        if(decorateeDescriptor.ImplementationFactory != null)
        {
            decorateeDescriptor =
                ServiceDescriptor.Describe(
                    serviceType: decorateeImplType,
                    decorateeDescriptor.ImplementationFactory,
                    decorateeDescriptor.Lifetime);
        }
        else if(decorateeDescriptor.ImplementationInstance != null)
        {
            decorateeDescriptor =
                ServiceDescriptor.Singleton(
                    serviceType: decorateeImplType,
                    decorateeDescriptor.ImplementationInstance);
        }
        else
        {
            decorateeDescriptor =
                ServiceDescriptor.Describe(
                    decorateeImplType,
                    decorateeImplType,
                    decorateeDescriptor.Lifetime);
        }

        return decorateeDescriptor;
    }

    private static Type GetImplementationType(this ServiceDescriptor serviceDescriptor)
    {
        if (serviceDescriptor.ImplementationType != null)
        {
            return serviceDescriptor.ImplementationType;
        }

        if (serviceDescriptor.ImplementationInstance != null)
        {
            return serviceDescriptor.ImplementationInstance.GetType();
        }

        if (serviceDescriptor.ImplementationFactory != null)
        {
            return serviceDescriptor.ImplementationFactory.GetType().GenericTypeArguments[1];
        }

        throw new InvalidOperationException("Cannot get the decoratee implementation type");
    }

    public static void AddImplementationsFromAssembly(
        this IServiceCollection services,
        Assembly assembly,
        Type interfaceType,
        ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        foreach (Type type in assembly.DefinedTypes.Where(type => !type.IsAbstract && type.IsClass && !type.IsGenericType))
        {
            services.Add(new ServiceDescriptor(type, type, lifetime));

            foreach(Type concreteInterfaceType in type.GetInterfaces()
                .Where(concreteInterfaceType => concreteInterfaceType.IsGenericType && interfaceType.IsAssignableFrom(concreteInterfaceType.GetGenericTypeDefinition())))
            {
                services.Add(new ServiceDescriptor(concreteInterfaceType, type, lifetime));
            }

            Type baseType = type.BaseType!.IsGenericType ? type.BaseType.GetGenericTypeDefinition() : type.BaseType;
            if(baseType == interfaceType)
            {
                services.Add(new ServiceDescriptor(type.BaseType, type, lifetime));
            }
        }
    }

    public static void AddDecorator<TService, TDecorator>(
        this IServiceCollection serviceCollection,
        Action<IServiceCollection> configureDecorateeServices)
        where TDecorator : class, TService
        where TService : class
    {
        ServiceCollection decorateeServices = new ServiceCollection();
        configureDecorateeServices(decorateeServices);

        ServiceDescriptor? decorateeDescriptor = decorateeServices.SingleOrDefault(sd => sd.ServiceType == typeof(TService));
        if(decorateeDescriptor == null)
        {
            throw new InvalidOperationException("No decoratee configured");
        }

        decorateeServices.Remove(decorateeDescriptor);

        ObjectFactory decoratorInstanceFactory = ActivatorUtilities.CreateFactory(
            typeof(TDecorator),
            new[] { typeof(TService) });

        Type decorateeImplType = decorateeDescriptor.GetImplementationType();
        Func<IServiceProvider, TDecorator> decoratorFactory = serviceProvider =>
        {
            object decoratee = serviceProvider.GetRequiredService(decorateeImplType);
            TDecorator decorator = (TDecorator)decoratorInstanceFactory(serviceProvider, new[] { decoratee });

            return decorator;
        };

        ServiceDescriptor decoratorDescriptor = ServiceDescriptor.Describe(
            typeof(TService),
            decoratorFactory,
            decorateeDescriptor.Lifetime);

        decorateeDescriptor = RefactorDecorateeDescriptor(decorateeDescriptor);

        serviceCollection.Add(decorateeDescriptor);
        serviceCollection.Add(decoratorDescriptor);
    }
}
