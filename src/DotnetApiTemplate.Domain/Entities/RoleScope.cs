using DotnetApiTemplate.Shared.Abstractions.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotnetApiTemplate.Domain.Entities;

public sealed class RoleScope : BaseEntity
{
    [ForeignKey(nameof(Role))]
    public Guid RoleId { get; set; }
    public Role? Role { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime? RevokedAt { get; set; }
    public string? RevokedMessage { get; set; }
}