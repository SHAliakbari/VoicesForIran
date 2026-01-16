using VoicesForIran.Core.Models;

namespace VoicesForIran.Core.Interfaces;

/// <summary>
/// Provides email templates loaded from files with random selection
/// </summary>
public interface ITemplateProvider
{
    /// <summary>
    /// Gets a random template from the available templates
    /// </summary>
    EmailTemplate GetRandomTemplate();

    /// <summary>
    /// Loads all templates from the template files
    /// </summary>
    Task LoadTemplatesAsync(CancellationToken cancellationToken = default);
}
