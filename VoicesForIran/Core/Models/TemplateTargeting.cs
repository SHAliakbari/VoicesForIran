namespace VoicesForIran.Core.Models;

/// <summary>
/// Targeting criteria for email templates.
/// Used to match templates with appropriate representatives.
/// </summary>
public sealed record TemplateTargeting
{
    /// <summary>
    /// The government level this template is designed for.
    /// Null means the template applies to all levels.
    /// </summary>
    public RepresentativeLevel? Level { get; init; }

    /// <summary>
    /// The political ideology/party group this template is targeted at.
    /// Null means the template applies to all ideologies.
    /// </summary>
    public PoliticalIdeology? Ideology { get; init; }

    /// <summary>
    /// The language this template is written in (e.g., "en", "fr").
    /// </summary>
    public string Language { get; init; } = "en";

    /// <summary>
    /// Optional tags for additional categorization (e.g., "security", "human-rights", "refugees").
    /// </summary>
    public List<string> Tags { get; init; } = [];

    /// <summary>
    /// Checks if this targeting matches the given representative criteria
    /// </summary>
    public bool Matches(RepresentativeLevel? level, PoliticalIdeology? ideology, string language)
    {
        // Level must match if specified
        if (Level.HasValue && level.HasValue && Level.Value != level.Value)
            return false;

        // Ideology must match if specified
        if (Ideology.HasValue && ideology.HasValue && Ideology.Value != ideology.Value)
            return false;

        // Language must match
        if (!string.Equals(Language, language, StringComparison.OrdinalIgnoreCase))
            return false;

        return true;
    }

    /// <summary>
    /// Calculates a specificity score for ranking template matches.
    /// Higher score = more specific match.
    /// </summary>
    public int GetSpecificityScore()
    {
        var score = 0;
        if (Level.HasValue) score += 2;
        if (Ideology.HasValue) score += 2;
        if (Tags.Count > 0) score += 1;
        return score;
    }
}
