# Voices for Iran

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Blazor](https://img.shields.io/badge/Blazor-Server-512BD4?logo=blazor)](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

**Empowering Iranian-Canadians to advocate for human rights and democracy in Iran.**

Voices for Iran is an open-source Blazor Server application that helps Canadian citizens easily contact their elected representatives about human rights issues in Iran. Simply enter your postal code, and the app will find your representatives and generate a personalized advocacy email.

## ✨ Features

- **📍 Postal Code Lookup** – Automatically finds your federal and provincial representatives based on your Canadian postal code
- **✉️ Email Generation** – Creates personalized advocacy emails using randomized templates to avoid spam filters
- **📊 Impact Dashboard** – Track community engagement with real-time statistics on emails generated and ridings represented
- **🔒 Privacy First** – We never store your name, postal code, or any personal information
- **📱 Mobile Responsive** – Full Bootstrap 5 responsive design works on any device

## 🚀 Getting Started

### Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) or later

### Running Locally

1. **Clone the repository**
   ```bash
   git clone https://github.com/SHAliakbari/VoicesForIran.git
   cd VoicesForIran
   ```

2. **Run the application**
   ```bash
   dotnet run --project VoicesForIran
   ```

3. **Open your browser**
   Navigate to `https://localhost:5001` or `http://localhost:5000`

## 📁 Project Structure

```
VoicesForIran/
├── Components/
│   ├── Layout/         # Main layout and navigation
│   ├── Pages/          # Razor pages (Home, Impact)
├── Core/
│   ├── Interfaces/     # Service contracts
│   ├── Models/         # Domain models
│   ├── Services/       # Business logic
├── Templates/          # Email templates (JSON)
├── wwwroot/            # Static assets (CSS, JS)
```

## 📨 Email Templates

The application uses randomized email templates to help avoid spam detection. Templates are stored as JSON files in the `Templates/` folder and support dynamic placeholders:

| Placeholder | Description |
|-------------|-------------|
| `{{RepresentativeTitle}}` | Full title with name (e.g., "MP John Smith") |
| `{{RepresentativeName}}` | Representative's name only |
| `{{RidingName}}` | Electoral district name |
| `{{PostalCode}}` | User's postal code |
| `{{UserName}}` | User's name (if provided) |

See [Templates/README.md](VoicesForIran/Templates/README.md) for more details on creating custom templates.

## 🛠 Tech Stack

- **Framework:** [.NET 10](https://dotnet.microsoft.com/) / [Blazor Server](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
- **UI:** [Bootstrap 5](https://getbootstrap.com/) with [Bootstrap Icons](https://icons.getbootstrap.com/)
- **Database:** SQLite (via Microsoft.Data.Sqlite)
- **Architecture:** Clean Architecture with dependency injection

## 🤝 Contributing

Contributions are welcome! Here's how you can help:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

### Ideas for Contributions

- 🌍 Add support for more provinces/territories
- 🇫🇷 Internationalization (French language support)
- 📧 Additional email templates
- ✅ Unit and integration tests
- 📝 Documentation improvements

## 🔒 Privacy

Voices for Iran is designed with privacy as a core principle:

- **No personal data storage** – Your name and postal code are never saved
- **Minimal logging** – Only aggregate statistics (riding name, MP name) are logged for the impact dashboard
- **Client-side email** – Emails open in your default mail client; we never see or send your emails

## 📜 License

This project is open source and available under the [MIT License](LICENSE).

## 🙏 Acknowledgments

- All contributors and supporters of human rights in Iran
- The Iranian-Canadian community for their continued advocacy

---

<p align="center">
  <strong>Your voice matters. Make it heard.</strong>
</p>

<p align="center">
  <a href="https://github.com/SHAliakbari/VoicesForIran">⭐ Star this repo</a> •
  <a href="https://github.com/SHAliakbari/VoicesForIran/issues">🐛 Report Bug</a> •
  <a href="https://github.com/SHAliakbari/VoicesForIran/issues">🚀 Request Feature</a>
</p>
