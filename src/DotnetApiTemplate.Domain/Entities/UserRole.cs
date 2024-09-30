using DotnetApiTemplate.Shared.Abstractions.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotnetApiTemplate.Domain.Entities;

public sealed class UserRole : BaseEntity, IEntity
{
    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }
    [ForeignKey(nameof(Role))]
    public Guid RoleId { get; set; }
    public Role? Role { get; set; }
    public User? User { get; set; }
}