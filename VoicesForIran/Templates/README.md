# Email Templates

This folder contains email templates in JSON format. The system randomly selects a template for each email generation to help avoid spam filter detection.

## Template Format

Each template is a JSON file with the following structure:

```json
{
  "Subject": "Your email subject line",
  "Body": "Your email body content with {{Placeholders}}"
}
```

## Available Placeholders

Use double curly braces `{{VariableName}}` for placeholders. The following variables are automatically replaced:

| Placeholder | Description | Example |
|-------------|-------------|---------|
| `{{RepresentativeTitle}}` | Full title with name | "MP John Smith" |
| `{{RepresentativeName}}` | Representative's name only | "John Smith" |
| `{{RepresentativeOffice}}` | Elected office title | "MP", "MPP", "Mayor" |
| `{{RidingName}}` | Electoral district name | "Ottawa Centre" |
| `{{PostalCode}}` | User's postal code (uppercase) | "K1A 0A6" |
| `{{UserName}}` | User's name (if provided) | "Jane Doe" |

## Adding New Templates

1. Create a new `.json` file in this folder
2. Follow the format above
3. Use placeholders where appropriate
4. The system will automatically load it on next startup

## Example Template

```json
{
  "Subject": "Constituent from {{PostalCode}}: Support Human Rights in Iran",
  "Body": "Dear {{RepresentativeTitle}},\n\nI am writing to you as a constituent from {{RidingName}} (postal code {{PostalCode}})...\n\nSincerely,\n{{UserName}}"
}
```

## Notes

- Templates are loaded at application startup
- If no templates exist, default templates are created automatically
- Use `\n` for newlines in JSON
- Placeholders are case-insensitive
