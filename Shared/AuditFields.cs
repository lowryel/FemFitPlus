using System;

namespace FemFitPlus.Shared;

public class BaseEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
}

public class AuditFields: BaseEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
}
