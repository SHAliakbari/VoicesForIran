using System.ComponentModel.DataAnnotations;

namespace VoicesForIran.Components.Home;

public sealed class EmailFormModel
{
    [Required(ErrorMessage = "Postal code is required")]
    [RegularExpression(
        @"^[ABCEGHJKLMNPRSTVXYabceghj-nprstv-y]\d[ABCEGHJKLMNPRSTVWXYZabceghj-nprstv-z]\s?\d[ABCEGHJKLMNPRSTVWXYZabceghj-nprstv-z]\d$",
        ErrorMessage = "Please enter a valid Canadian postal code (e.g., K1A 0A6)")]
    public string? PostalCode { get; set; }

    public string? UserName { get; set; }
}
