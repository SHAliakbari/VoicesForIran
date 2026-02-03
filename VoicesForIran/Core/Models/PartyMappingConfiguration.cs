using System.Text.Json.Serialization;

namespace VoicesForIran.Core.Models;

/// <summary>
/// Configuration for mapping party names to political ideologies.
/// Loaded from party-mapping.json configuration file.
/// </summary>
public sealed class PartyMappingConfiguration
{
    /// <summary>
    /// Dictionary mapping ideology names to their party lists
    /// </summary>
    [JsonPropertyName("ideologies")]
    public Dictionary<string, IdeologyGroup> Ideologies { get; set; } = [];

    /// <summary>
    /// Gets the ideology for a given party name
    /// </summary>
    public PoliticalIdeology GetIdeology(string? partyName)
    {
        if (string.IsNullOrWhiteSpace(partyName))
            return PoliticalIdeology.NonPartisan;

        foreach (var (ideologyKey, group) in Ideologies)
        {
            if (group.Parties.Any(p => p.Equals(partyName, StringComparison.OrdinalIgnoreCase)))
            {
                return Enum.TryParse<PoliticalIdeology>(ideologyKey, ignoreCase: true, out var ideology)
                    ? ideology
                    : PoliticalIdeology.Independent;
            }
        }

        return PoliticalIdeology.Independent;
    }
}

/// <summary>
/// Represents a group of parties under a single ideology
/// </summary>
public sealed class IdeologyGroup
{
    /// <summary>
    /// Display name for the ideology group
    /// </summary>
    [JsonPropertyName("displayName")]
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// List of party names that belong to this ideology
    /// </summary>
    [JsonPropertyName("parties")]
    public List<string> Parties { get; set; } = [];
}
