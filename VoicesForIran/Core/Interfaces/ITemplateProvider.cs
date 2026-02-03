using VoicesForIran.Core.Models;

namespace VoicesForIran.Core.Interfaces;

/// <summary>
/// Provides email templates loaded from files with random and targeted selection
/// </summary>
public interface ITemplateProvider
{
    /// <summary>
    /// Gets a random template from the available templates (legacy method for backward compatibility)
    /// </summary>
    EmailTemplate GetRandomTemplate();

    /// <summary>
    /// Gets a targeted template matching the representative's characteristics.
    /// Falls back to less specific templates if no exact match found.
    /// </summary>
    /// <param name="representative">The representative to target</param>
    /// <returns>The best matching template</returns>
    EmailTemplate GetTargetedTemplate(Representative representative);

    /// <summary>
    /// Gets all templates matching the specified criteria
    /// </summary>
    /// <param name="level">Optional government level filter</param>
    /// <param name="ideology">Optional political ideology filter</param>
    /// <param name="language">Language code (defaults to "en")</param>
    /// <returns>List of matching templates</returns>
    IReadOnlyList<EmailTemplate> GetTemplates(RepresentativeLevel? level = null, PoliticalIdeology? ideology = null, string language = "en");

    /// <summary>
    /// Gets the political ideology for a given party name
    /// </summary>
    /// <param name="partyName">The party name to look up</param>
    /// <returns>The corresponding political ideology</returns>
    PoliticalIdeology GetIdeologyForParty(string? partyName);

    /// <summary>
    /// Loads all templates from the template files
    /// </summary>
    Task LoadTemplatesAsync(CancellationToken cancellationToken = default);
}
