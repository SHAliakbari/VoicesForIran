using VoicesForIran.Core.Models;

namespace VoicesForIran.Core.Interfaces;

/// <summary>
/// Generates mailto: links for advocacy emails
/// </summary>
public interface IMailtoGenerator
{
    /// <summary>
    /// Generates a mailto: URI with the MP as TO and other representatives as CC.
    /// Recipients include name and title (e.g., "John Smith, MP" &lt;john@email.com&gt;)
    /// </summary>
    /// <param name="lookupResult">The representative lookup result</param>
    /// <param name="template">The email template to use</param>
    /// <param name="userName">Optional user name for signature</param>
    /// <returns>A properly encoded mailto: URI string</returns>
    string GenerateMailtoLink(RepresentativeLookupResult lookupResult, EmailTemplate template, string? userName = null);
}
