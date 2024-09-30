using DotnetApiTemplate.Shared.Abstractions.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotnetApiTemplate.Domain.Entities;

public sealed class UserDevice : BaseEntity
{
    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public string? DeviceId { get; set; }
    public string? FcmToken { get; set; }
    public DateTime? FcmTokenExpiredAt { get; set; }
}