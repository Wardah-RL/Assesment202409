using DotnetApiTemplate.Shared.Abstractions.Entities;

namespace DotnetApiTemplate.Domain.Entities;

public sealed class Role : BaseEntity
{
    public bool IsDefault { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public ICollection<RoleScope> RoleScopes { get; set; } = new HashSet<RoleScope>();
}