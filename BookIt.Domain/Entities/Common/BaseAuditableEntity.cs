﻿namespace BookIt.Domain.Entities.Common;

public abstract class BaseAuditableEntity : BaseEntity
{
    public string CreatedBy { get; set; } = null!;
    public string UpdatedBy { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;

}
