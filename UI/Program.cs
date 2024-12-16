using Domain;
using Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace UI;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        var services = new ServiceCollection();
        
        services.AddTransient<MainArea>();
        services.RegisterDomain();
        services.RegisterLogger();
        
        var serviceProvider = services.BuildServiceProvider();
        
        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        
        ApplicationConfiguration.Initialize();
        var mainForm = serviceProvider.GetRequiredService<MainArea>();
        Application.Run(mainForm);
    }
}