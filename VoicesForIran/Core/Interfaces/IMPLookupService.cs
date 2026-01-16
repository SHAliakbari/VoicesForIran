using VoicesForIran.Core.Models;

namespace VoicesForIran.Core.Interfaces;

/// <summary>
/// Service to look up elected representatives by postal code
/// </summary>
public interface IMPLookupService
{
    /// <summary>
    /// Fetches representatives for the given Canadian postal code
    /// </summary>
    /// <param name="postalCode">Canadian postal code (e.g., "K1A 0A6" or "K1A0A6")</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Lookup result containing all representatives for the postal code</returns>
    Task<RepresentativeLookupResult> LookupByPostalCodeAsync(string postalCode, CancellationToken cancellationToken = default);
}
