using System.Text.Json.Serialization;

namespace VoicesForIran.Infrastructure.Api;

/// <summary>
/// API response models for the Open North Represent API
/// https://represent.opennorth.ca/
/// </summary>

public sealed class RepresentApiResponse
{
    [JsonPropertyName("representatives_centroid")]
    public List<RepresentativeDto>? RepresentativesCentroid { get; set; }

    [JsonPropertyName("representatives_concordance")]
    public List<RepresentativeDto>? RepresentativesConcordance { get; set; }

    [JsonPropertyName("boundaries_centroid")]
    public List<BoundaryDto>? BoundariesCentroid { get; set; }

    [JsonPropertyName("boundaries_concordance")]
    public List<BoundaryDto>? BoundariesConcordance { get; set; }

    /// <summary>
    /// Gets all representatives from both centroid and concordance lookups
    /// </summary>
    public IEnumerable<RepresentativeDto> GetAllRepresentatives()
    {
        var all = new List<RepresentativeDto>();

        if (RepresentativesCentroid is not null)
            all.AddRange(RepresentativesCentroid);

        if (RepresentativesConcordance is not null)
            all.AddRange(RepresentativesConcordance);

        // Deduplicate by name and elected_office
        return all
            .GroupBy(r => (r.Name, r.ElectedOffice))
            .Select(g => g.First());
    }
}

public sealed class RepresentativeDto
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("elected_office")]
    public string? ElectedOffice { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("district_name")]
    public string? DistrictName { get; set; }

    [JsonPropertyName("party_name")]
    public string? PartyName { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("personal_url")]
    public string? PersonalUrl { get; set; }

    [JsonPropertyName("photo_url")]
    public string? PhotoUrl { get; set; }

    [JsonPropertyName("representative_set_name")]
    public string? RepresentativeSetName { get; set; }

    [JsonPropertyName("first_name")]
    public string? FirstName { get; set; }

    [JsonPropertyName("last_name")]
    public string? LastName { get; set; }

    [JsonPropertyName("offices")]
    public List<OfficeDto>? Offices { get; set; }
}

public sealed class OfficeDto
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("tel")]
    public string? Tel { get; set; }

    [JsonPropertyName("fax")]
    public string? Fax { get; set; }
}

public sealed class BoundaryDto
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("boundary_set_name")]
    public string? BoundarySetName { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("external_id")]
    public string? ExternalId { get; set; }
}
