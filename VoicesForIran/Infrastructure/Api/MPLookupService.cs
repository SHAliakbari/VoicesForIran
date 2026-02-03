using System.Net.Http.Json;
using System.Text.RegularExpressions;
using VoicesForIran.Core.Interfaces;
using VoicesForIran.Core.Models;

namespace VoicesForIran.Infrastructure.Api;

/// <summary>
/// Service to look up elected representatives using the Open North Represent API
/// </summary>
public sealed partial class MPLookupService : IMPLookupService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<MPLookupService> _logger;

    private const string BaseUrl = "https://represent.opennorth.ca";

    public MPLookupService(HttpClient httpClient, ILogger<MPLookupService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<RepresentativeLookupResult> LookupByPostalCodeAsync(string postalCode, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(postalCode);

        // Normalize postal code: remove spaces and convert to uppercase
        var normalizedPostalCode = NormalizePostalCode(postalCode);

        if (!IsValidCanadianPostalCode(normalizedPostalCode))
        {
            _logger.LogWarning("Invalid postal code format: {PostalCode}", postalCode);
            return new RepresentativeLookupResult
            {
                PostalCode = postalCode,
                Representatives = []
            };
        }

        try
        {
            var url = $"{BaseUrl}/postcodes/{normalizedPostalCode}/";
            _logger.LogInformation("Looking up representatives for postal code: {PostalCode}", normalizedPostalCode);

            var response = await _httpClient.GetFromJsonAsync<RepresentApiResponse>(url, cancellationToken);

            if (response is null)
            {
                _logger.LogWarning("No response received for postal code: {PostalCode}", normalizedPostalCode);
                return new RepresentativeLookupResult
                {
                    PostalCode = postalCode,
                    Representatives = []
                };
            }

            var representatives = response.GetAllRepresentatives()
                .Where(r => !string.IsNullOrWhiteSpace(r.Name) && !string.IsNullOrWhiteSpace(r.ElectedOffice))
                .Select(MapToRepresentative)
                .OrderByDescending(r => r.IsFederalMP) // MPs first
                .ThenBy(r => r.ElectedOffice)
                .ToList();

            _logger.LogInformation("Found {Count} representatives for postal code: {PostalCode}",
                representatives.Count, normalizedPostalCode);

            return new RepresentativeLookupResult
            {
                PostalCode = postalCode,
                Representatives = representatives
            };
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error looking up representatives for postal code: {PostalCode}", normalizedPostalCode);
            throw new InvalidOperationException($"Failed to look up representatives for postal code: {postalCode}", ex);
        }
    }

    private static Representative MapToRepresentative(RepresentativeDto dto) => new()
    {
        Name = dto.Name ?? "Unknown",
        ElectedOffice = dto.ElectedOffice ?? "Unknown",
        Email = string.IsNullOrWhiteSpace(dto.Email) ? null : dto.Email.Trim(),
        DistrictName = dto.DistrictName,
        Party = dto.PartyName,
        Url = dto.PersonalUrl ?? dto.Url,
        Gender = dto.Gender,
        PreferredLanguages = dto.Extra?.PreferredLanguages is { Count: > 0 }
            ? string.Join("  ", dto.Extra.PreferredLanguages)
            : null
    };

    private static string NormalizePostalCode(string postalCode)
    {
        // Remove all whitespace and convert to uppercase
        return PostalCodeWhitespaceRegex().Replace(postalCode, "").ToUpperInvariant();
    }

    private static bool IsValidCanadianPostalCode(string postalCode)
    {
        // Canadian postal code format: A1A1A1 (letter-digit-letter-digit-letter-digit)
        // First letter cannot be D, F, I, O, Q, U, W, Z
        return CanadianPostalCodeRegex().IsMatch(postalCode);
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex PostalCodeWhitespaceRegex();

    [GeneratedRegex(@"^[ABCEGHJ-NPRSTVXY]\d[ABCEGHJ-NPRSTV-Z]\d[ABCEGHJ-NPRSTV-Z]\d$")]
    private static partial Regex CanadianPostalCodeRegex();
}
