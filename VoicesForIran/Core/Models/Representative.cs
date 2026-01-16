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
    /// Indicates if this is a federal MP (Member of Parliament)
    /// </summary>
    public bool IsFederalMP => ElectedOffice.Contains("MP", StringComparison.OrdinalIgnoreCase) ||
                               ElectedOffice.Contains("Member of Parliament", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Indicates if this representative has a valid email address
    /// </summary>
    public bool HasEmail => !string.IsNullOrWhiteSpace(Email);
}
