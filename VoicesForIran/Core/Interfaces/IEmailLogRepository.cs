using VoicesForIran.Core.Models;

namespace VoicesForIran.Core.Interfaces;

/// <summary>
/// Repository for logging email generation (Impact Dashboard only)
/// Privacy: Only stores MP name, riding name, and timestamp - NO user data
/// </summary>
public interface IEmailLogRepository
{
    /// <summary>
    /// Logs that an email was generated for a specific MP/riding
    /// </summary>
    Task LogEmailGenerationAsync(string mpName, string ridingName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets impact statistics for the dashboard
    /// </summary>
    Task<ImpactStats> GetImpactStatsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets recent email generation logs
    /// </summary>
    Task<IReadOnlyList<EmailGenerationLog>> GetRecentLogsAsync(int count = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Initializes the database schema
    /// </summary>
    Task InitializeAsync(CancellationToken cancellationToken = default);
}
