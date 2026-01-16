namespace VoicesForIran.Core.Models;

/// <summary>
/// Pre-defined email template for advocacy messages.
/// Loaded from template files with placeholder support.
/// </summary>
public sealed record EmailTemplate
{
    public required string FileName { get; init; }
    public required string Subject { get; init; }
    public required string Body { get; init; }

    /// <summary>
    /// Processes the template by replacing placeholders with actual values
    /// </summary>
    /// <param name="variables">Dictionary of placeholder names and their values</param>
    /// <returns>A new EmailTemplate with placeholders replaced</returns>
    public EmailTemplate WithVariables(Dictionary<string, string> variables)
    {
        var processedSubject = ReplacePlaceholders(Subject, variables);
        var processedBody = ReplacePlaceholders(Body, variables);

        return this with
        {
            Subject = processedSubject,
            Body = processedBody
        };
    }

    private static string ReplacePlaceholders(string text, Dictionary<string, string> variables)
    {
        var result = text;
        foreach (var (key, value) in variables)
        {
            result = result.Replace($"{{{{{key}}}}}", value, StringComparison.OrdinalIgnoreCase);
        }
        return result;
    }
}
