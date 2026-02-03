using System.Text.Json;
using VoicesForIran.Core.Interfaces;
using VoicesForIran.Core.Models;

namespace VoicesForIran.Core.Services;

/// <summary>
/// Provides email templates loaded from JSON files in the Templates folder.
/// Supports targeted selection based on representative level, party, and language.
/// Randomly selects from matching templates to help avoid spam filter patterns.
/// </summary>
public sealed class TemplateProvider : ITemplateProvider
{
    private readonly string _templatesPath;
    private readonly ILogger<TemplateProvider> _logger;
    private readonly Random _random = new();
    private List<EmailTemplate> _templates = [];
    private PartyMappingConfiguration _partyMapping = new();

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };

    public TemplateProvider(IWebHostEnvironment environment, ILogger<TemplateProvider> logger)
    {
        _templatesPath = Path.Combine(environment.ContentRootPath, "Templates");
        _logger = logger;
    }

    public async Task LoadTemplatesAsync(CancellationToken cancellationToken = default)
    {
        _templates.Clear();

        // Ensure templates directory exists
        if (!Directory.Exists(_templatesPath))
        {
            _logger.LogError("Templates directory not found at {Path}. Please ensure template files exist.", _templatesPath);
            throw new DirectoryNotFoundException($"Templates directory not found at {_templatesPath}");
        }

        // Load party mapping configuration
        await LoadPartyMappingAsync(cancellationToken);

        // Load all templates recursively
        await LoadTemplatesFromDirectoryAsync(_templatesPath, cancellationToken);

        if (_templates.Count == 0)
        {
            _logger.LogError("No templates found in {Path}. Please ensure template JSON files exist in the Templates directory and its subdirectories.", _templatesPath);
            throw new InvalidOperationException($"No templates found in {_templatesPath}. Please ensure template JSON files exist in the Templates directory and its subdirectories.");

        }

        _logger.LogInformation("Loaded {Count} email templates", _templates.Count);
    }

    private async Task LoadPartyMappingAsync(CancellationToken cancellationToken)
    {
        var configPath = Path.Combine(_templatesPath, "config", "party-mapping.json");

        if (!File.Exists(configPath))
        {
            _logger.LogWarning("Party mapping configuration not found at {Path}. Using empty mapping.", configPath);
            _partyMapping = new PartyMappingConfiguration();
            return;
        }

        try
        {
            var json = await File.ReadAllTextAsync(configPath, cancellationToken);
            _partyMapping = JsonSerializer.Deserialize<PartyMappingConfiguration>(json, JsonOptions) ?? new();
            _logger.LogDebug("Loaded party mapping with {Count} ideology groups", _partyMapping.Ideologies.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load party mapping configuration from {Path}", configPath);
            _partyMapping = new PartyMappingConfiguration();
        }
    }

    private async Task LoadTemplatesFromDirectoryAsync(string directory, CancellationToken cancellationToken)
    {
        // Skip config directory
        if (directory.EndsWith("config", StringComparison.OrdinalIgnoreCase))
            return;

        var templateFiles = Directory.GetFiles(directory, "*.json");

        foreach (var file in templateFiles)
        {
            try
            {
                var json = await File.ReadAllTextAsync(file, cancellationToken);
                var templateData = JsonSerializer.Deserialize<TemplateFileData>(json, JsonOptions);

                if (templateData is not null && !string.IsNullOrWhiteSpace(templateData.Subject) && !string.IsNullOrWhiteSpace(templateData.Body))
                {
                    var relativePath = Path.GetRelativePath(_templatesPath, file);
                    var targeting = ParseTargetingFromPath(relativePath, templateData.Targeting);

                    _templates.Add(new EmailTemplate
                    {
                        Id = templateData.Id ?? Path.GetFileNameWithoutExtension(file),
                        FileName = relativePath,
                        Subject = templateData.Subject,
                        Body = templateData.Body,
                        Targeting = targeting
                    });
                    _logger.LogDebug("Loaded template: {FileName} (Level={Level}, Ideology={Ideology}, Language={Language})",
                        relativePath, targeting.Level, targeting.Ideology, targeting.Language);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load template file: {File}", file);
            }
        }

        // Recursively load from subdirectories
        foreach (var subDir in Directory.GetDirectories(directory))
        {
            await LoadTemplatesFromDirectoryAsync(subDir, cancellationToken);
        }
    }

    private static TemplateTargeting ParseTargetingFromPath(string relativePath, TemplateTargetingData? explicitTargeting)
    {
        var pathParts = relativePath.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

        // Initialize from explicit targeting if provided
        RepresentativeLevel? level = explicitTargeting?.Level;
        PoliticalIdeology? ideology = explicitTargeting?.Ideology;
        var language = explicitTargeting?.Language ?? "en";
        var tags = explicitTargeting?.Tags ?? [];

        // Parse from path structure: Templates/[lang]/[level]/[ideology]/filename.json
        foreach (var part in pathParts)
        {
            var lowerPart = part.ToLowerInvariant();

            // Language detection
            if (lowerPart is "en" or "fr")
            {
                language = lowerPart;
                continue;
            }

            // Level detection
            if (lowerPart == "federal")
            {
                level = RepresentativeLevel.Federal;
                continue;
            }
            if (lowerPart == "provincial")
            {
                level = RepresentativeLevel.Provincial;
                continue;
            }
            if (lowerPart == "municipal")
            {
                level = RepresentativeLevel.Municipal;
                continue;
            }

            // Ideology detection
            if (Enum.TryParse<PoliticalIdeology>(part, ignoreCase: true, out var parsedIdeology))
            {
                ideology = parsedIdeology;
            }
        }

        return new TemplateTargeting
        {
            Level = level,
            Ideology = ideology,
            Language = language,
            Tags = tags
        };
    }

    public EmailTemplate GetRandomTemplate()
    {
        if (_templates.Count == 0)
        {
            throw new InvalidOperationException("No templates loaded. Call LoadTemplatesAsync first.");
        }

        var index = _random.Next(_templates.Count);
        return _templates[index];
    }

    public EmailTemplate GetTargetedTemplate(Representative representative)
    {
        if (_templates.Count == 0)
        {
            throw new InvalidOperationException("No templates loaded. Call LoadTemplatesAsync first.");
        }

        var level = representative.Level;
        var ideology = GetIdeologyForParty(representative.Party);
        var language = representative.PreferredLanguageCode;

        // Try to find templates with decreasing specificity
        var candidates = FindMatchingTemplates(level, ideology, language);

        if (candidates.Count == 0)
        {
            // Fallback to any template in the same language
            candidates = _templates.Where(t =>
                string.Equals(t.Targeting.Language, language, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(t.Targeting.Language, "en", StringComparison.OrdinalIgnoreCase)).ToList();
        }

        if (candidates.Count == 0)
        {
            // Ultimate fallback to any template
            candidates = _templates;
        }

        // Randomly select from matching templates
        var index = _random.Next(candidates.Count);
        var selected = candidates[index];

        _logger.LogDebug("Selected template {Template} for {Representative} (Level={Level}, Ideology={Ideology}, Language={Language})",
            selected.FileName, representative.Name, level, ideology, language);

        return selected;
    }

    private List<EmailTemplate> FindMatchingTemplates(RepresentativeLevel level, PoliticalIdeology ideology, string language)
    {
        // Priority 1: Exact match (level + ideology + language)
        var exact = _templates.Where(t =>
            t.Targeting.Level == level &&
            t.Targeting.Ideology == ideology &&
            string.Equals(t.Targeting.Language, language, StringComparison.OrdinalIgnoreCase)).ToList();

        if (exact.Count > 0) return exact;

        // Priority 2: Level + language (any ideology or no ideology specified)
        var levelMatch = _templates.Where(t =>
            t.Targeting.Level == level &&
            (!t.Targeting.Ideology.HasValue || t.Targeting.Ideology == ideology) &&
            string.Equals(t.Targeting.Language, language, StringComparison.OrdinalIgnoreCase)).ToList();

        if (levelMatch.Count > 0) return levelMatch;

        // Priority 3: Ideology + language (any level or no level specified)
        var ideologyMatch = _templates.Where(t =>
            t.Targeting.Ideology == ideology &&
            (!t.Targeting.Level.HasValue || t.Targeting.Level == level) &&
            string.Equals(t.Targeting.Language, language, StringComparison.OrdinalIgnoreCase)).ToList();

        if (ideologyMatch.Count > 0) return ideologyMatch;

        // Priority 4: Generic templates for the language
        var genericMatch = _templates.Where(t =>
            !t.Targeting.Level.HasValue &&
            !t.Targeting.Ideology.HasValue &&
            string.Equals(t.Targeting.Language, language, StringComparison.OrdinalIgnoreCase)).ToList();

        if (genericMatch.Count > 0) return genericMatch;

        // Priority 5: Any English template as ultimate fallback
        return _templates.Where(t =>
            string.Equals(t.Targeting.Language, "en", StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public IReadOnlyList<EmailTemplate> GetTemplates(RepresentativeLevel? level = null, PoliticalIdeology? ideology = null, string language = "en")
    {
        return _templates.Where(t =>
            (!level.HasValue || !t.Targeting.Level.HasValue || t.Targeting.Level == level) &&
            (!ideology.HasValue || !t.Targeting.Ideology.HasValue || t.Targeting.Ideology == ideology) &&
            string.Equals(t.Targeting.Language, language, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public PoliticalIdeology GetIdeologyForParty(string? partyName)
    {
        return _partyMapping.GetIdeology(partyName);
    }

    /// <summary>
    /// Internal class for deserializing template JSON files
    /// </summary>
    private sealed class TemplateFileData
    {
        public string? Id { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public TemplateTargetingData? Targeting { get; set; }
    }

    /// <summary>
    /// Internal class for deserializing targeting data from template JSON files
    /// </summary>
    private sealed class TemplateTargetingData
    {
        public RepresentativeLevel? Level { get; set; }
        public PoliticalIdeology? Ideology { get; set; }
        public string? Language { get; set; }
        public List<string> Tags { get; set; } = [];
    }
}
