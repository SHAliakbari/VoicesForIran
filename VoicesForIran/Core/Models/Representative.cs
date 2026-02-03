namespace VoicesForIran.Core.Models;

/// <summary>
/// Represents an elected representative (MP, MPP, Mayor, Councillor, etc.)
/// </summary>
public sealed record Representative
{
    public required string Name { get; init; }
    public required string ElectedOffice { get; init; }
    public string? Email { get; init; }
    public string? DistrictName { get; init; }
    public string? Party { get; init; }
    public string? Url { get; init; }

    /// <summary>
    /// Gender of the representative (M, F, or empty/null if unknown)
    /// </summary>
    public string? Gender { get; init; }

    /// <summary>
    /// Preferred languages for communication (e.g., "English", "French", "English  French")
    /// </summary>
    public string? PreferredLanguages { get; init; }

    /// <summary>
    /// Indicates if this is a federal MP (Member of Parliament)
    /// </summary>
    public bool IsFederalMP => (ElectedOffice.Contains("MP", StringComparison.OrdinalIgnoreCase) &&
                            !ElectedOffice.Contains("MPP", StringComparison.OrdinalIgnoreCase)) ||
                           (ElectedOffice.Contains("Member of Parliament", StringComparison.OrdinalIgnoreCase) &&
                            !ElectedOffice.Contains("MPP", StringComparison.OrdinalIgnoreCase));


    /// <summary>
    /// Indicates if this is a provincial representative (MPP, MLA, MNA, MHA)
    /// </summary>
    public bool IsProvincial => ElectedOffice.Contains("MPP", StringComparison.OrdinalIgnoreCase) ||
                                ElectedOffice.Contains("MLA", StringComparison.OrdinalIgnoreCase) ||
                                ElectedOffice.Contains("MNA", StringComparison.OrdinalIgnoreCase) ||
                                ElectedOffice.Contains("MHA", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Indicates if this is a municipal representative (Mayor, Councillor, Regional Councillor, etc.)
    /// </summary>
    public bool IsMunicipal => ElectedOffice.Contains("Mayor", StringComparison.OrdinalIgnoreCase) ||
                               ElectedOffice.Contains("Councillor", StringComparison.OrdinalIgnoreCase) ||
                               ElectedOffice.Contains("Chair", StringComparison.OrdinalIgnoreCase) ||
                               ElectedOffice.Contains("Reeve", StringComparison.OrdinalIgnoreCase) ||
                               ElectedOffice.Contains("Warden", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Gets the government level for this representative
    /// </summary>
    public RepresentativeLevel Level => IsFederalMP ? RepresentativeLevel.Federal
                                       : IsProvincial ? RepresentativeLevel.Provincial
                                       : RepresentativeLevel.Municipal;

    /// <summary>
    /// Indicates if this representative prefers French communication
    /// </summary>
    public bool PrefersFrench => PreferredLanguages?.Contains("French", StringComparison.OrdinalIgnoreCase) == true;


    /// <summary>
    /// Gets the preferred language code ("en" or "fr")
    /// </summary>
    public string PreferredLanguageCode => PrefersFrench ? "fr" : "en";

    /// <summary>
    /// Indicates if this representative has a valid email address
    /// </summary>
    public bool HasEmail => !string.IsNullOrWhiteSpace(Email);

    /// <summary>
    /// Gets the appropriate honorific based on gender
    /// </summary>
    public string Honorific => Gender?.ToUpperInvariant() switch
    {
        "M" => "Mr.",
        "F" => "Ms.",
        _ => ""
    };
}
