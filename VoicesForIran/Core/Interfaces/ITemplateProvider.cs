using VoicesForIran.Core.Models;

namespace VoicesForIran.Core.Interfaces;

/// <summary>
/// Provides email templates loaded from files with random and targeted selection
/// </summary>
public interface ITemplateProvider
{
    /// <summary>
    /// Loads all templates from the template files
    /// </summary>
    Task LoadTemplatesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a targeted template matching the representative's characteristics.
    /// Falls back to less specific templates if no exact match found.
    /// </summary>
    /// <param name="representative">The representative to target</param>
    /// <param name="language">The language for the template.</param>
    /// <returns>A randomly selected matching template, or null if none found.</returns>
    EmailTemplate? GetTemplate(Representative representative, string language = "en");

    /// <summary>
    /// Gets all loaded templates.
    /// </summary>
    /// <returns>A list of all email templates.</returns>
    IEnumerable<EmailTemplate> GetAllTemplates();

    /// <summary>
    /// Gets a specific template by its ID.
    /// </summary>
    /// <param name="templateId">The ID of the template to retrieve.</param>
    /// <returns>The email template with the specified ID, or null if not found.</returns>
    EmailTemplate? GetTemplateById(string templateId);

    /// <summary>
    /// Gets a random template from the available templates.
    /// </summary>
    EmailTemplate GetRandomTemplate();

    /// <summary>
    /// Gets all templates matching the specified criteria.
    /// </summary>
    /// <param name="level">Optional government level filter</param>
    /// <param name="ideology">Optional political ideology filter</param>
    /// <param name="language">Language code (defaults to "en")</param>
    /// <returns>List of matching templates</returns>
    IReadOnlyList<EmailTemplate> GetTemplates(RepresentativeLevel? level = null, PoliticalIdeology? ideology = null, string language = "en");

    /// <summary>
    /// Gets the political ideology for a given party name.
    /// </summary>
    /// <param name="partyName">The party name to look up</param>
    /// <returns>The corresponding political ideology</returns>
    PoliticalIdeology GetIdeologyForParty(string? partyName);
}

