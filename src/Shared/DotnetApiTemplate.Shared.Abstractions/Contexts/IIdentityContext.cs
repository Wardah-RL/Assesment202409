namespace DotnetApiTemplate.Shared.Abstractions.Contexts;

public interface IIdentityContext
{
    bool IsAuthenticated { get; }
    public Guid Id { get; }
    public string Username { get; }
    public string? FullName { get; }
    Dictionary<string, IEnumerable<string>> Claims { get; }
    List<string> Roles { get; }
}