namespace VoicesForIran.Core.Models;

/// <summary>
/// Political ideology groupings that map to specific party names.
/// Used for targeted template selection.
/// </summary>
public enum PoliticalIdeology
{
    /// <summary>
    /// Conservative parties (Conservative, PC, UCP, Saskatchewan Party, etc.)
    /// </summary>
    Conservative,

    /// <summary>
    /// Liberal parties (Liberal, Ontario Liberal, BC Liberal, etc.)
    /// </summary>
    Liberal,

    /// <summary>
    /// NDP/Social Democratic parties (NDP, provincial NDP variants, Québec solidaire)
    /// </summary>
    Ndp,

    /// <summary>
    /// Green parties (Federal Green, provincial Green parties)
    /// </summary>
    Green,

    /// <summary>
    /// Quebec nationalist parties (Bloc Québécois, CAQ, PQ)
    /// </summary>
    Bloc,

    /// <summary>
    /// Independent or other parties that don't fit major categories
    /// </summary>
    Independent,

    /// <summary>
    /// Non-partisan representatives (typically municipal level)
    /// </summary>
    NonPartisan
}
