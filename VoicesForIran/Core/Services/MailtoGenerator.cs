using System.Text;
using VoicesForIran.Core.Interfaces;
using VoicesForIran.Core.Models;

namespace VoicesForIran.Core.Services;

/// <summary>
/// Generates properly encoded mailto: links for advocacy emails.
/// Formats recipients with name and title (e.g., "John Smith, MP" &lt;john@email.com&gt;)
/// </summary>
public sealed class MailtoGenerator : IMailtoGenerator
{
    public string GenerateMailtoLink(RepresentativeLookupResult lookupResult, EmailTemplate template, string? userName = null)
    {
        var primaryRecipient = lookupResult.PrimaryMP ?? lookupResult.EmailableRepresentatives.FirstOrDefault();

        if (primaryRecipient?.Email is null)
        {
            throw new InvalidOperationException("No representatives with email addresses found.");
        }

        var ccRecipients = lookupResult.EmailableRepresentatives
            .Where(r => r != primaryRecipient)
            .ToList();

        return GenerateMailtoLink(primaryRecipient, ccRecipients, template, userName);
    }

    private static string GenerateMailtoLink(Representative toRecipient, IEnumerable<Representative> ccRecipients, EmailTemplate template, string? userName)
    {
        var sb = new StringBuilder();
        sb.Append("mailto:");
        sb.Append(FormatRecipient(toRecipient));

        var parameters = new List<string>();

        // Add CC recipients with names and titles
        var ccList = ccRecipients.Where(r => r.HasEmail).ToList();
        if (ccList.Count > 0)
        {
            var ccString = string.Join(",", ccList.Select(FormatRecipient));
            parameters.Add($"cc={ccString}");
        }

        // Add subject
        parameters.Add($"subject={Uri.EscapeDataString(template.Subject)}");

        // Add body with optional signature
        var body = FormatBodyWithSignature(template.Body, userName);
        parameters.Add($"body={Uri.EscapeDataString(body)}");

        if (parameters.Count > 0)
        {
            sb.Append('?');
            sb.Append(string.Join("&", parameters));
        }

        return sb.ToString();
    }

    /// <summary>
    /// Formats a recipient as "Name (Title)" &lt;email@example.com&gt;
    /// Uses parentheses instead of comma to avoid breaking mailto recipient list
    /// </summary>
    private static string FormatRecipient(Representative rep)
    {
        // Format: "John Smith (MP)" <john.smith@parl.gc.ca>
        // Note: Cannot use comma as it's the recipient separator in mailto
        var displayName = $"{rep.Name} ({rep.ElectedOffice})";
        var formatted = $"\"{displayName}\" <{rep.Email}>";
        return Uri.EscapeDataString(formatted);
    }

    private static string FormatBodyWithSignature(string body, string? userName)
    {
        return body;
    }
}
