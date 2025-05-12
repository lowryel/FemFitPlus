using System;

namespace FemFitPlus.Models.Dtos;

public class SubscriptionCreateDto
{
    public string? UserId { get; set; }
    public string? SubscriptionType { get; set; } // Monthly, Yearly, etc.
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; } = true;
    public string? PaymentMethod { get; set; } // Credit Card, PayPal, etc.
}
