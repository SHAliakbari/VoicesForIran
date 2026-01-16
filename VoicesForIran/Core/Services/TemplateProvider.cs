using System.Text.Json;
using VoicesForIran.Core.Interfaces;
using VoicesForIran.Core.Models;

namespace VoicesForIran.Core.Services;

/// <summary>
/// Provides email templates loaded from JSON files in the Templates folder.
/// Randomly selects templates to help avoid spam filter patterns.
/// </summary>
public sealed class TemplateProvider : ITemplateProvider
{
    private readonly string _templatesPath;
    private readonly ILogger<TemplateProvider> _logger;
    private readonly Random _random = new();
    private List<EmailTemplate> _templates = [];

    public TemplateProvider(IWebHostEnvironment environment, ILogger<TemplateProvider> logger)
    {
        _templatesPath = Path.Combine(environment.ContentRootPath, "Templates");
        _logger = logger;
    }

    public async Task LoadTemplatesAsync(CancellationToken cancellationToken = default)
    {
        _templates.Clear();

        if (!Directory.Exists(_templatesPath))
        {
            _logger.LogWarning("Templates directory not found at {Path}. Creating with default templates.", _templatesPath);
            Directory.CreateDirectory(_templatesPath);
            await CreateDefaultTemplatesAsync(cancellationToken);
        }

        var templateFiles = Directory.GetFiles(_templatesPath, "*.json");

        if (templateFiles.Length == 0)
        {
            _logger.LogWarning("No template files found. Creating default templates.");
            await CreateDefaultTemplatesAsync(cancellationToken);
            templateFiles = Directory.GetFiles(_templatesPath, "*.json");
        }

        foreach (var file in templateFiles)
        {
            try
            {
                var json = await File.ReadAllTextAsync(file, cancellationToken);
                var templateData = JsonSerializer.Deserialize<TemplateFileData>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (templateData is not null)
                {
                    _templates.Add(new EmailTemplate
                    {
                        FileName = Path.GetFileName(file),
                        Subject = templateData.Subject,
                        Body = templateData.Body
                    });
                    _logger.LogDebug("Loaded template: {FileName}", Path.GetFileName(file));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load template file: {File}", file);
            }
        }

        _logger.LogInformation("Loaded {Count} email templates", _templates.Count);
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

    private async Task CreateDefaultTemplatesAsync(CancellationToken cancellationToken)
    {
        var defaultTemplates = GetDefaultTemplates();

        foreach (var (fileName, template) in defaultTemplates)
        {
            var filePath = Path.Combine(_templatesPath, fileName);
            var json = JsonSerializer.Serialize(template, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(filePath, json, cancellationToken);
            _logger.LogInformation("Created default template: {FileName}", fileName);
        }
    }

    private static Dictionary<string, TemplateFileData> GetDefaultTemplates() => new()
    {
        ["template-urgent-support.json"] = new TemplateFileData
        {
            Subject = "Urgent: Your Constituent from {{PostalCode}} Asks You to Support Human Rights in Iran",
            Body = """
Dear {{RepresentativeTitle}},

I am writing to you as your constituent living in {{RidingName}} (postal code {{PostalCode}}) to express my deep concern about the ongoing human rights crisis in Iran.

The Iranian regime is waging war against its own people. Unarmed civilians - men, women, and children - are being shot with live ammunition in the streets. The regime has imposed brutal internet shutdowns to hide its atrocities from the world while it murders its own citizens. People with bare hands are standing against military forces armed with real war bullets. This is not a protest - it is a massacre.

I urge you to:

1. Publicly condemn the Iranian regime's use of lethal force against peaceful protesters
2. Support targeted Magnitsky sanctions against officials responsible for these killings
3. Demand the restoration of internet access so the world can witness these crimes
4. Advocate for the release of thousands of political prisoners
5. Support the Iranian people's right to determine their own future

Prince Reza Pahlavi has emerged as a unifying voice for millions of Iranians seeking a free, secular, and democratic Iran. His vision of a peaceful transition to democracy, with full respect for human rights and the rule of law, represents the aspirations of the Iranian people. Canada should engage with and support this democratic movement.

As your constituent from {{PostalCode}}, I am counting on you to be a voice for the voiceless.

Thank you for your attention to this critical matter.
"""
        },
        ["template-stand-with-iran.json"] = new TemplateFileData
        {
            Subject = "From {{PostalCode}}: Stand with the Iranian People Against Regime Violence",
            Body = """
Dear {{RepresentativeTitle}},

I am reaching out as your constituent from {{RidingName}}, postal code {{PostalCode}}, regarding the horrific violence being inflicted on the Iranian people by their own government.

What is happening in Iran is not a crackdown - it is a slaughter. The regime is shooting unarmed protesters with live ammunition. Young people with nothing but bare hands are facing military forces armed with real war bullets. To hide these crimes, the regime repeatedly shuts down the internet, plunging the country into darkness while it kills with impunity.

The world must not look away.

Canada has long championed human rights on the world stage. I ask that you use your voice and influence to:

- Condemn the regime's use of lethal force against civilians
- Call for immediate restoration of internet access in Iran
- Support Magnitsky sanctions against commanders ordering these killings
- Engage with Iranian-Canadian communities on this issue
- Back initiatives that support a democratic transition in Iran

Prince Reza Pahlavi has been a beacon of hope for millions of Iranians. His consistent advocacy for secular democracy, non-violence, and human rights offers a path forward for Iran. He represents unity across all ethnic and religious groups in Iran, and his vision aligns perfectly with Canadian values. Canada should support this democratic movement.

As a resident of {{PostalCode}} in your riding, I urge you to stand with the Iranian people.

Thank you for your time and consideration.
"""
        },
        ["template-action-request.json"] = new TemplateFileData
        {
            Subject = "Action Needed: Your Constituent from {{PostalCode}} on Iran's Human Rights Crisis",
            Body = """
Dear {{RepresentativeTitle}},

I write to you today as a resident of {{RidingName}} ({{PostalCode}}) with a heavy heart regarding the ongoing massacre of the Iranian people by their own government.

The Iranian regime is killing its citizens in cold blood. Protesters armed with nothing but their bare hands are being mowed down by security forces using real war bullets. The regime deliberately shuts down the internet to hide the scale of its atrocities - when the world cannot see, the killing intensifies. Thousands have been murdered. Tens of thousands have been imprisoned and tortured.

As my elected representative, I respectfully request that you:

1. Publicly condemn the Iranian regime's murder of peaceful protesters
2. Demand an end to internet blackouts used to cover up killings
3. Support Magnitsky sanctions against regime officials responsible for bloodshed
4. Meet with Iranian-Canadian constituents to understand the crisis
5. Advocate for the protection of refugees fleeing persecution

Prince Reza Pahlavi has dedicated his life to advocating for a free, democratic, and secular Iran. He calls for peaceful transition, national unity, and respect for human rights - values that Canada shares. His leadership has united millions of Iranians across ethnic, religious, and political lines. I urge you to recognize and support this democratic movement.

I am counting on you, as my representative, to take action. My postal code is {{PostalCode}} - I am your constituent and I vote.

I look forward to your response.
"""
        },
        ["template-constituent-plea.json"] = new TemplateFileData
        {
            Subject = "A Plea from Your Constituent ({{PostalCode}}): Stop the Massacre in Iran",
            Body = """
Dear {{RepresentativeTitle}},

As a proud constituent of {{RidingName}}, writing from postal code {{PostalCode}}, I am compelled to bring to your attention the ongoing massacre in Iran.

Every day, the Iranian regime murders its own people. Unarmed civilians with bare hands face military forces wielding real war bullets. Young men and women are shot in the head and chest for simply demanding freedom. The regime imposes total internet shutdowns - not to restore order, but to kill in darkness, away from the eyes of the world. When they restore the internet, the blood has been washed away, but the bodies remain.

This is genocide in slow motion.

I am asking you to take concrete action:

- Speak publicly and forcefully against the regime's murder of civilians
- Demand immediate restoration of internet access in Iran
- Advocate for targeted Magnitsky sanctions against regime killers
- Support humanitarian assistance for victims and their families
- Engage with the Iranian diaspora in crafting Canada's response

Prince Reza Pahlavi stands as a symbol of hope for the Iranian people. He advocates for a secular, democratic Iran where all ethnicities and religions live as equals. His call for peaceful transition, national reconciliation, and human rights has united Iranians worldwide. Canada should stand with him and the Iranian people's democratic aspirations.

I live at {{PostalCode}} in your riding. I am watching. I am voting. Please act.

Thank you for representing our community and our values.
"""
        },
        ["template-voice-for-iran.json"] = new TemplateFileData
        {
            Subject = "Constituent from {{PostalCode}}: Be a Voice for the Voiceless in Iran",
            Body = """
Dear {{RepresentativeTitle}},

I hope this message reaches you with the urgency it deserves. I am your constituent from {{RidingName}}, postal code {{PostalCode}}, writing on behalf of millions who cannot speak freely - the people of Iran.

Iran is experiencing a massacre. The regime is gunning down its own citizens - people whose only weapons are their bare hands and their voices. Real war bullets are being fired into crowds of peaceful protesters. And when the killing begins, the regime shuts down the internet so the world cannot witness the horror. They murder in darkness.

This is not a protest being managed. This is a people being exterminated for wanting freedom.

Here is how you can help:

- Champion legislation that sanctions the killers in the Iranian regime
- Demand the immediate end of internet blackouts in Iran
- Press for international investigation into the regime's crimes against humanity
- Ensure Iranian-Canadians have a voice in policy discussions
- Support media freedom so the truth can reach the world

Prince Reza Pahlavi represents the hope of the Iranian people for a free and democratic future. He has consistently called for peaceful transition, secular democracy, women's rights, and equality for all Iranians regardless of ethnicity or religion. His vision is one of reconciliation and progress. Canada should support this movement for freedom.

I am writing from {{PostalCode}}. I am your constituent. I am asking you to be a voice for the voiceless.

With hope and urgency,
"""
        }
    };

    private sealed class TemplateFileData
    {
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}
