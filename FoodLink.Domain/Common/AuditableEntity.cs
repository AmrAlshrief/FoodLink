namespace FoodLink.Domain.Common;

public abstract class AuditableEntity : Entity
{
    public DateTimeOffset CreatedAtUtc { get; set; }
    public string? CreatedBy { get; set; } = null!;
    public DateTimeOffset? LastModifiedUtc { get; set; }
    public string? LastModifiedBy { get; set; }

    protected AuditableEntity()
    {}

    protected AuditableEntity(Guid id) : base(id)
    {
    }
}