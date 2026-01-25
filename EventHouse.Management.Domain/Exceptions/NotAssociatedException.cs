namespace EventHouse.Management.Domain.Exceptions;

public sealed class NotAssociatedException(
    string parent,
    string child,
    Guid parentId,
    Guid childId) : DomainException($"{parent} '{parentId}' is not associated with {child} '{childId}'.")
{
    public string Parent { get; } = parent;
    public string Child { get; } = child;
    public Guid ParentId { get; } = parentId;
    public Guid ChildId { get; } = childId;
}
