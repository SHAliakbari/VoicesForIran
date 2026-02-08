# Email Templates for Iran's Lion-and-Sun Revolution

This folder contains intelligent, context-aware email templates that advocate for Iran's Lion-and-Sun Revolution. Templates are automatically selected based on the representative's government level, political party, and language preference, ensuring realistic and appropriate calls-to-action for each recipient.

## Folder Structure

```
Templates/
??? party-mapping.json          # Maps party names to ideology groups
??? en/                         # English templates
?   ??? generic-stand-with-iran.json     # Generic templates
?   ??? generic-iran-solidarity.json
?   ??? federal/                # Federal MPs (can influence international policy)
?   ?   ??? conservative/       # 3 templates (IRGC, PS752, strong leadership)
?   ?   ??? liberal/            # 2 templates (HR leadership, intl cooperation)
?   ?   ??? ndp/                # 3 templates (workers solidarity, refugee compassion, grassroots)
?   ?   ??? green/              # 2 templates (protest rights, grassroots power)
?   ?   ??? bloc/               # 3 templates (Quebec values, solidarity, community)
?   ?   ??? independent/        # 3 templates (non-partisan HR focus)
?   ??? provincial/             # Provincial MPPs/MLAs (settlement services, resolutions)
?   ?   ??? conservative/       # 1 template (community support)
?   ?   ??? liberal/            # 1 template (refugee welcome)
?   ?   ??? ndp/                # 1 template (solidarity action)
?   ?   ??? green/              # 3 templates (solidarity variations)
?   ?   ??? independent/        # 3 templates (non-partisan solidarity)
?   ??? municipal/              # Municipal representatives (local solidarity only)
?       ??? nonpartisan/        # 3 templates (councillor, mayor, regional)
??? fr/                         # French templates
?   ??? francais-general.json   # General French template
?   ??? francais-solidarite.json
?   ??? francais-urgence.json
??? template-*.json             # Root-level generic templates
```

## Template Format

Each template is a JSON file with this structure:

```json
{
  "Subject": "Your email subject line with {{Placeholders}}",
  "Body": "Email body with \\n\\n for line breaks and {{Placeholders}}"
}
```

**Note:** Targeting is automatically inferred from the folder structure (government level and party). No separate `Targeting` object is needed.

## Available Placeholders

Use double curly braces `{{VariableName}}` for placeholders:

| Placeholder | Description | Example |
|-------------|-------------|---------|
| `{{RepresentativeFullName}}` | Name with honorific | "Mr. John Smith" or "John Smith" |
| `{{RepresentativeName}}` | Name only | "John Smith" |
| `{{RidingOrRegion}}` | Electoral district/region | "Ottawa Centre" or "your riding" |
| `{{PostalCode}}` | User's postal code (uppercase) | "K1A 0A6" |
| `{{UserName}}` | User's name | "Jane Doe" or "A concerned constituent" |
| `{{PartyName}}` | Political party name | "Conservative" |
| `{{Honorific}}` | Gender-based honorific | "Mr.", "Ms.", "" (empty if unknown) |

**Legacy placeholders** (still supported for backward compatibility):
- `{{RepresentativeTitle}}` ? maps to `{{RepresentativeFullName}}`
- `{{RidingName}}` ? maps to `{{RidingOrRegion}}`
- `{{RepresentativeOffice}}` ? office title like "MP", "MPP"

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
| `Federal` | MPs (Members of Parliament) - Can influence international policy |
| `Provincial` | MPPs, MLAs, MNAs, MHAs - Settlement services, resolutions, call on feds |
| `Municipal` | Mayors, Councillors - Local solidarity, community support only |

## Core Message Elements

**All templates must include:**

1. **Opening**: "I am a concerned constituent living in {{PostalCode}} in your {{RidingOrRegion}}."
2. **Lion-and-Sun Revolution**: Reference to Iran's Lion-and-Sun Revolution and the people's fight for freedom
3. **Context**: Mention brutal repression (mass killings of tens of thousands, torture, executions, internet blackouts)
4. **Six Key Demands** (for federal templates):
   - Dismantle the regime's machinery of repression
   - Cut off the regime's financial lifelines
   - Ensure free internet and communications
   - Expel regime "diplomats" and prosecute criminals
   - Immediate release of all political prisoners
   - Recognize a legitimate transitional government
5. **Canada-Specific Actions** (federal only):
   - Remove IRGC members from Canada (~700 reported)
   - Seize regime assets for humanitarian aid
   - Invite Prince Reza Pahlavi to address Parliament
6. **Closing Slogan**: "Long live freedom. Long live Iran. Victory to Iran's Lion-and-Sun Revolution."

## Template Guidelines by Party and Level

### Federal Conservative Templates
**Focus**: Security threats, IRGC terrorism, PS752, strong accountability

**Key phrases**:
- "IRGC terrorist designation" and removal of members from Canada
- "Flight PS752" - 55 Canadians killed
- "Strength and moral clarity"
- "Canada must not be a safe haven"

**CTAs**: Enforce IRGC designation, expand sanctions, seize assets, PS752 justice, invite Pahlavi to Parliament

### Federal Liberal Templates
**Focus**: Human rights leadership, international cooperation, feminist foreign policy

**Key phrases**:
- "Canada's feminist foreign policy"
- "International coordination" and "diplomatic leadership"
- "Women-led revolution"
- "Canadian values and reputation"

**CTAs**: Work with allies, UN mechanisms, refugee pathways, women-centered approaches, invite Pahlavi

### Federal NDP Templates
**Focus**: Workers' rights, refugee compassion, solidarity with oppressed

**Key phrases**:
- "Workers, teachers, nurses" - regime targets labor
- "Refugee pathways and settlement services"
- "Grassroots movements"
- "Right side of history"

**CTAs**: Support Iranian workers, expand refugee acceptance, sanctions on regime (not people), grassroots support

### Federal Green Templates
**Focus**: Protest rights, grassroots democracy, civil liberties

**Key phrases**:
- "Right to peaceful protest"
- "Grassroots civil society"
- "Environmental and social activists"
- "People-led movements"

**CTAs**: Defend protest rights, support activists, ensure sanctions target regime elite, recognize transitional government

### Federal Bloc Québécois Templates
**Focus**: Quebec values (secularism, laïcité), human rights, community

**Key phrases**:
- "Quebec values" - secularism, individual freedom
- "Religious persecution"
- "Community solidarity"

**CTAs**: Stand with Quebec values, support refugees fleeing religious persecution, federal action

### Provincial Templates (All Parties)
**Scope**: Provincial jurisdiction only - NO international policy

**Focus**: Settlement services, community support, calling on federal government

**CTAs**:
- Pass provincial resolutions of solidarity
- Enhance refugee settlement and integration services
- Mental health support for trauma survivors
- Expedite credential recognition
- Call on federal government to act
- Support Iranian-Canadian community organizations

**DO NOT include**: Expelling diplomats, recognizing governments, international sanctions

### Municipal Templates (Nonpartisan)
**Scope**: Municipal jurisdiction only - NO provincial or federal policy

**Focus**: Local community solidarity and practical support

**CTAs**:
- Pass council resolutions of solidarity
- Fly Lion-and-Sun flag at municipal buildings
- Community events recognizing Iranian people's struggle
- Ensure newcomer services accessible to Iranian refugees
- Support local Iranian-Canadian organizations
- Issue proclamations

**DO NOT include**: Any international or provincial policy items

## Party Mapping Configuration

The `party-mapping.json` file maps all Canadian party names to ideology groups:

```json
{
  "conservative": ["Conservative", "Progressive Conservative Party of Ontario", "United Conservative Party", ...],
  "liberal": ["Liberal", "Ontario Liberal Party", "BC United", ...],
  "ndp": ["NDP", "New Democratic Party", "Québec solidaire", ...],
  "green": ["Green Party", "Green Party of Ontario", ...],
  "bloc": ["Bloc Québécois", "Coalition Avenir Québec", "Parti Québécois"],
  "independent": ["Independent", "Forces et Démocratie", ...]
}
```

## Writing New Templates

### ? DO:
- Start with the required opening line
- Tailor CTAs to the representative's actual powers
- Use party-specific framing and values
- Reference Prince Reza Pahlavi for federal templates
- Include the closing slogan
- Keep tone respectful and urgent
- Use 200-400 words
- Use only ASCII characters (hyphens, not em-dashes)

### ? DON'T:
- Include voting references ("I voted for you")
- Ask municipal reps about international policy
- Ask provincial reps to expel diplomats
- Use partisan endorsements
- Include specific date references
- Use em-dashes (—) or curly quotes (" ") - causes encoding issues
- Make unrealistic requests based on level of government

## Example Templates

### Federal Template (Conservative)
```
Subject: Security Threat: Act Against Iranian Regime - IRGC in Canada
Body: Focus on IRGC terrorism, PS752, security threat to Canada, removing IRGC members
CTAs: Enforce designation, sanctions, seize assets, prosecute criminals, invite Pahlavi
```

### Provincial Template (Liberal)
```
Subject: Welcome Iranian Refugees: Provincial Action Needed
Body: Focus on refugees fleeing persecution, provincial services needed
CTAs: Expand settlement services, health care, credential recognition, call on feds
```

### Municipal Template (Nonpartisan)
```
Subject: Support Our Iranian-Canadian Community
Body: Focus on local community grieving, need for solidarity
CTAs: Council resolution, fly flag, community events, ensure services accessible
```

## Adding New Templates

1. Create a new `.json` file in the appropriate folder (e.g., `en/federal/conservative/`)
2. Follow the template format above
3. Use placeholders where appropriate
4. Include all core message elements
5. Ensure CTAs are appropriate for the government level
6. Use only ASCII characters (no em-dashes or curly quotes)
7. Test the template by placing it in the Templates folder - it will automatically load

## Technical Notes

- All templates must be valid JSON
- Use `\\n\\n` for line breaks in the Body field
- Files are encoded as UTF-8 without BOM
- Templates are loaded at application startup
- Selection is deterministic but randomized to avoid spam detection
