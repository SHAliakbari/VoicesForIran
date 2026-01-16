using VoicesForIran.Core.Interfaces;
using VoicesForIran.Core.Services;
using VoicesForIran.Infrastructure.Api;
using VoicesForIran.Infrastructure.Data;

namespace VoicesForIran.Infrastructure;

/// <summary>
/// Extension methods for registering infrastructure services
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds all infrastructure services to the DI container
    /// </summary>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        // Core services
        services.AddSingleton<ITemplateProvider, TemplateProvider>();
        services.AddSingleton<IMailtoGenerator, MailtoGenerator>();

        // HTTP client for MP lookup
        services.AddHttpClient<IMPLookupService, MPLookupService>(client =>
        {
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("User-Agent", "VoicesForIran/1.0");
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        // SQLite repository for email logging
        services.AddSingleton<IEmailLogRepository, SqliteEmailLogRepository>();

        return services;
    }

    /// <summary>
    /// Initializes the database and other infrastructure components
    /// </summary>
    public static async Task InitializeInfrastructureAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        
        // Initialize database
        var logRepository = scope.ServiceProvider.GetRequiredService<IEmailLogRepository>();
        await logRepository.InitializeAsync();

        // Load email templates from files
        var templateProvider = scope.ServiceProvider.GetRequiredService<ITemplateProvider>();
        await templateProvider.LoadTemplatesAsync();
    }
}
