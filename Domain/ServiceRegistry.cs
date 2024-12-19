using System.Diagnostics.CodeAnalysis;
using Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Domain;

[ExcludeFromCodeCoverage]
public static class ServiceRegistry
{
    public static IServiceCollection RegisterDomain(this IServiceCollection services)
    {
        services.AddTransient<IIndividLifecycleService, IndividLifecycleService>();
        services.AddTransient<IDescendantFactory, DescendantFactory>();
        services.AddSingleton<IRandomProvider, RandomProvider>();
        
        return services;
    }
}