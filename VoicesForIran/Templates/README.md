# Email Templates

This folder contains targeted email templates in JSON format. The system selects templates based on the representative's government level, political party, and preferred language.

## Folder Structure

```
Templates/
??? config/
?   ??? party-mapping.json      # Maps party names to ideology groups
??? en/                         # English templates
?   ??? federal/                # Federal MPs
?   ?   ??? conservative/       # Conservative Party templates
?   ?   ??? liberal/            # Liberal Party templates
?   ?   ??? ndp/                # NDP templates
?   ?   ??? green/              # Green Party templates
?   ?   ??? bloc/               # Bloc Québécois templates
?   ?   ??? independent/        # Independent MPs
?   ??? provincial/             # Provincial MPPs/MLAs
?   ?   ??? conservative/
?   ?   ??? liberal/
?   ?   ??? ndp/
?   ?   ??? green/
?   ?   ??? independent/
?   ??? municipal/              # Municipal representatives
?       ??? nonpartisan/        # Mayors, Councillors (non-partisan)
??? fr/                         # French templates
    ??? federal/
    ??? provincial/
    ??? (generic French templates)
```

## Template Format

Each template is a JSON file with the following structure:

```json
{
  "Id": "optional-unique-id",
  "Subject": "Your email subject line with {{Placeholders}}",
  "Body": "Your email body content with {{Placeholders}}",
  "Targeting": {
    "Level": "Federal",
    "Ideology": "Conservative",
    "Language": "en",
    "Tags": ["security", "sanctions"]
  }
}
```

**Note:** The `Targeting` section is optional. If omitted, targeting is inferred from the folder structure.

## Available Placeholders

Use double curly braces `{{VariableName}}` for placeholders:

| Placeholder | Description | Example |
|-------------|-------------|---------|
| `{{RepresentativeTitle}}` | Full title with name | "MP John Smith" |
| `{{RepresentativeName}}` | Representative's name only | "John Smith" |
| `{{RepresentativeOffice}}` | Elected office title | "MP", "MPP", "Mayor" |
| `{{RidingName}}` | Electoral district name | "Ottawa Centre" |
| `{{PostalCode}}` | User's postal code (uppercase) | "K1A 0A6" |
| `{{UserName}}` | User's name (if provided) | "Jane Doe" |
| `{{Honorific}}` | Gender-based honorific | "Mr.", "Ms.", "" |

## Political Ideologies

Templates are organized by political ideology groups:

| Ideology | Federal Parties | Provincial Examples |
|----------|-----------------|---------------------|
| `conservative` | Conservative | PC Ontario, UCP Alberta, Sask Party |
| `liberal` | Liberal | Ontario Liberal, BC Liberal, NS Liberal |
| `ndp` | NDP | All provincial NDP variants, Québec solidaire |
| `green` | Green Party | All provincial Green parties |
| `bloc` | Bloc Québécois | CAQ, PQ (Quebec nationalist) |
| `independent` | Independent | Various independents and small parties |
| `nonpartisan` | N/A | Municipal representatives (mayors, councillors) |

## Template Selection Priority

When selecting a template for a representative, the system uses this priority:

1. **Exact match**: Level + Ideology + Language
2. **Level match**: Level + Language (any ideology)
3. **Ideology match**: Ideology + Language (any level)
4. **Generic**: Language only
5. **Fallback**: Any English template

## Government Levels

| Level | Representatives |
|-------|-----------------|
| `Federal` | MPs (Members of Parliament) |
| `Provincial` | MPPs, MLAs, MNAs, MHAs |
| `Municipal` | Mayors, Councillors, Regional Councillors, Reeves, Wardens |

## Party Mapping Configuration

The `config/party-mapping.json` file maps all 36+ Canadian party names to ideology groups:

```json
{
  "ideologies": {
    "conservative": {
      "displayName": "Conservative",
      "parties": [
        "Conservative",
        "Progressive Conservative Party of Ontario",
        "United Conservative Party",
        ...
      ]
    },
    ...
  }
}
```

## Adding New Templates

1. Create a new `.json` file in the appropriate folder (e.g., `en/federal/conservative/`)
2. Follow the template format above
3. Use placeholders where appropriate
4. The system will automatically load it on next startup

## Messaging Guidelines

### Conservative Templates
- Focus on security threats, sanctions enforcement, accountability
- Reference IRGC terrorism designation, PS752
- Emphasize strength and decisive action

### Liberal Templates
- Focus on human rights leadership, international reputation
- Reference feminist foreign policy, Canadian values
- Emphasize diplomatic coordination

### NDP Templates
- Focus on solidarity with workers, refugees
- Reference labor rights, grassroots movements
- Emphasize humanitarian support

### Green Templates
- Focus on right to protest, activism solidarity
- Reference environmental/social movements
- Emphasize peaceful change

### Bloc Templates
- Focus on Quebec values, secularism
- Reference human rights, individual freedom
- Bilingual considerations

### Municipal Templates
- Focus on local community support
- Less policy-specific, more solidarity-focused
- Appropriate for mayors and councillors
