using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Infrastructure;

public static class ServiceRegistry
{
    public static IServiceCollection RegisterLogger(this IServiceCollection services)
    {
        var filePath = Directory.GetCurrentDirectory();
        filePath = Path.Combine(filePath, "logs.txt");

        Log.Logger = new LoggerConfiguration()
            .WriteTo.File(filePath)
            .CreateLogger();

        services.AddSingleton<ILogger>(Log.Logger);
        
        LogWatcher.WatchLogs(filePath);

        return services;
    }
}