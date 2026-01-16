namespace VoicesForIran.Core.Models;

/// <summary>
/// Log entry for email generation (for Impact Dashboard)
/// Privacy: NO user data stored - only MP/riding info
/// </summary>
public sealed record EmailGenerationLog
{
    public int Id { get; init; }
    public required string MpName { get; init; }
    public required string RidingName { get; init; }
    public required DateTime GeneratedAtUtc { get; init; }
}
