using System;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using FemFitPlus.Services.Filters;

namespace FemFitPlus.Shared;

public class Helpers
{
    // Helper method to validate phase values
    public static bool IsValidPhase(string phase)
    {
        // Define valid phases
        var validPhases = new[] { "Menstrual", "Follicular", "Ovulation", "Luteal" };
        return validPhases.Contains(phase, StringComparer.OrdinalIgnoreCase);
    }

    // Helper method to sanitize inputs
    public static string SanitizeInput(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        // Remove potential XSS vectors and sanitize input
        // Use a proper HTML sanitizer library for production
        return input
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Replace("\"", "&quot;")
            .Replace("'", "&#x27;")
            .Replace("/", "&#x2F;");
    }

    public static double? CalculateBmi(double weightKg, double? heightCm)
    {
        if (heightCm == null || heightCm <= 0)
            return null;

        var heightM = heightCm.Value / 100;
        return Math.Round(weightKg / (heightM * heightM), 2);
    }

    public static string BuildCacheKey(ProfileFilter filter)
    {
        var json = JsonSerializer.Serialize(filter);
        var hash = Convert.ToBase64String(
            SHA256.HashData(Encoding.UTF8.GetBytes(json)));

        return $"profiles:{hash}";
    }

}

// Extension method for ClaimsPrincipal to get user ID
public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal principal)
    {
        return principal.FindFirstValue(ClaimTypes.NameIdentifier)!;
    }
}

