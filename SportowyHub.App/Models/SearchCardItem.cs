using SportowyHub.Models.Api;

namespace SportowyHub.Models;

public record SearchCardItem(
    ListingSummary Listing,
    bool HasCondition,
    string? ConditionText,
    Color ConditionBadgeColor);
