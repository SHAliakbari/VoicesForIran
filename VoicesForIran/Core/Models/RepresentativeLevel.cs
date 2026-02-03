namespace VoicesForIran.Core.Models;

/// <summary>
/// Represents the level of government for an elected representative
/// </summary>
public enum RepresentativeLevel
{
    /// <summary>
    /// Federal level (House of Commons - MPs)
    /// </summary>
    Federal,

    /// <summary>
    /// Provincial/Territorial level (MPPs, MLAs, MNAs, MHAs)
    /// </summary>
    Provincial,

    /// <summary>
    /// Municipal/Regional level (Mayors, Councillors, Regional Councillors)
    /// </summary>
    Municipal
}
