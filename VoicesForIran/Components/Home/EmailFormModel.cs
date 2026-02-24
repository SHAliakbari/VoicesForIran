using System.ComponentModel.DataAnnotations;

namespace VoicesForIran.Components.Home;

public sealed class EmailFormModel
{
    [Required(ErrorMessage = "Postal code is required")]
    [RegularExpression(
        @"^[ABCEGHJKLMNPRSTVXYabceghj-nprstv-y]\d[ABCEGHJKLMNPRSTVWXYZabceghj-nprstv-z]\s?\d[ABCEGHJKLMNPRSTVWXYZabceghj-nprstv-z]\d$",
        ErrorMessage = "Please enter a valid Canadian postal code (e.g., K1A 0A6)")]
    public string? PostalCode { get; set; }

    [Required(ErrorMessage = "Please enter your full name.")]
    public string UserName { get; set; } = string.Empty;

    [RegularExpression(
        @"^(\+1[-.\s]?)?(\()?[2-9]\d{2}(\))?[-.\s]?\d{3}[-.\s]?\d{4}$",
        ErrorMessage = "Please enter a valid Canadian phone number (e.g., 416-555-0123)")]
    public string? PhoneNumber { get; set; }

    public string? SelectedTemplateId { get; set; }
}
