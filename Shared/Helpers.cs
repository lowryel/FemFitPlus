using System;
using System.Security.Claims;

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
}

// Extension method for ClaimsPrincipal to get user ID
public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal principal)
    {
        return principal.FindFirstValue(ClaimTypes.NameIdentifier)!;
    }
}