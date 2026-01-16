namespace VoicesForIran.Core.Models;

/// <summary>
/// Result of looking up representatives for a postal code
/// </summary>
public sealed record RepresentativeLookupResult
{
    public required string PostalCode { get; init; }
    public required IReadOnlyList<Representative> Representatives { get; init; }

    /// <summary>
    /// The primary federal MP to send the email TO
    /// </summary>
    public Representative? PrimaryMP => Representatives.FirstOrDefault(r => r.IsFederalMP && r.HasEmail);

    /// <summary>
    /// Other representatives to CC on the email
    /// </summary>
    public IReadOnlyList<Representative> CCRecipients => Representatives
        .Where(r => r.HasEmail && !r.IsFederalMP)
        .ToList();

    /// <summary>
    /// All representatives with valid email addresses
    /// </summary>
    public IReadOnlyList<Representative> EmailableRepresentatives => Representatives
        .Where(r => r.HasEmail)
        .ToList();

    public bool HasAnyRepresentatives => Representatives.Count > 0;
    public bool HasEmailableRepresentatives => EmailableRepresentatives.Count > 0;
}
