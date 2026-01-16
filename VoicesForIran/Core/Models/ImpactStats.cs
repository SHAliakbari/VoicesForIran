namespace VoicesForIran.Core.Models;

/// <summary>
/// Statistics for the Impact Dashboard
/// </summary>
public sealed record ImpactStats
{
    public required int TotalEmailsGenerated { get; init; }
    public required int UniqueRidings { get; init; }
    public required IReadOnlyList<RidingStats> TopRidings { get; init; }
}

public sealed record RidingStats
{
    public required string RidingName { get; init; }
    public required string MpName { get; init; }
    public required int EmailCount { get; init; }
}
