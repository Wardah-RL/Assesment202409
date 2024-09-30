using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.WebApi.Contracts.Responses;

namespace DotnetApiTemplate.WebApi.Mapping;

public static class DomainToApiContractMapper
{
    public static RoleResponse ToRoleResponse(this Role role)
    {
        var response = new RoleResponse
        {
            RoleId = role.Id,
            Name = role.Name,
            Description = role.Description
        };

        foreach (var roleScope in role.RoleScopes)
            response.Scopes.Add(roleScope.ToScopeResponse());

        return response;
    }

    public static ScopeResponse ToScopeResponse(this RoleScope roleScope)
    {
        return new ScopeResponse
        {
            ScopeId = roleScope.Id,
            Name = roleScope.Name
        };
    }

    public static UserResponse ToUserResponse(this User user)
    {
        var response = new UserResponse
        {
            UserId = user.Id,
            Username = user.Username,
            FullName = user.FullName,
            LastPasswordChangeAt = user.LastPasswordChangeAt,
            Email = user.Email,
        };
        response.Scopes.AddRange(user.UserRoles.Select(e => e.Role).SelectMany(e => e!.RoleScopes).Select(e => e.Name));
        return response;
    }
}