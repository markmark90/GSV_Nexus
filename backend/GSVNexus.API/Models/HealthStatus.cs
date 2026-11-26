namespace GSVNexus.API.Models;

public sealed record HealthStatus(
    string Status,
    string Database,
    DateTime CheckedAt);
