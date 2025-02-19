using DotNetAPI.Domain.Common.Interfaces;
using System.Security.Claims;

namespace DotNetAPI.Core.Common.Authorization.CurrentUserAuthorization;

internal class CurrentUser : ICurrentUser
{
    public static ICollection<string> EmptyList = new List<string>();

    private ICollection<string> _roles = EmptyList;
    private ICollection<string> _permissions = EmptyList;

    public int Id { get; private set; }

    public string UserName { get; private set; } = default!;

    public string Email { get; private set; } = default!;

    public string Type { get; private set; } = default!;

    public bool IsVerified { get; private set; } = default!;

    public bool IsAuthenticated { get; private set; } = default!;


    public IReadOnlyList<string> Roles => _roles.ToList();

    public IReadOnlyList<string> Permissions => _permissions.ToList();

    public CurrentUser(ICollection<string> roles, ICollection<string> permissions, int id, string userName, string email, string type, bool isVerified, bool isAuthenticated)
    {
        _roles = roles;
        _permissions = permissions;
        Id = id;
        UserName = userName.Trim().ToUpper();
        Email = email.Trim().ToUpper();
        Type = type.Trim().ToUpper();
        IsVerified = isVerified;
        IsAuthenticated = isAuthenticated;
    }

    public CurrentUser()
    {

    }

    public bool IsUnauthorized(bool WebsiteIsLocked)
    {
        if (IsAuthenticated && WebsiteIsLocked) //so we know that some user is logged in
        {
            if (!((Type.ToUpper() == "ADMIN" || Type.ToUpper() == "TESTER") && IsVerified == true))
            {
                return true;
            }
        }
        else if (WebsiteIsLocked)
        {
            return true;
        }

        return false;
    }

    public bool HasPermission(string permission)
    {
        return Permissions?.Contains(permission) ?? false;
    }

    public bool HasRole(string role)
    {
        return Roles?.Contains(role) ?? false;
    }

    public void Reset()
    {
        Id = default;
        UserName = string.Empty;
        Email = string.Empty;
        Type = string.Empty;
        IsVerified = false;
        IsAuthenticated = false;
        _roles = EmptyList;
        _permissions = EmptyList;
    }

    public void Fill(CurrentUser? currentUser)
    {
        if (currentUser == null)
        {
            return;
        }

        Id = currentUser.Id;
        UserName = currentUser.UserName;
        Email = currentUser.Email;
        Type = currentUser.Type;
        IsVerified = currentUser.IsVerified;
        IsAuthenticated = currentUser.IsAuthenticated;

        _roles = currentUser.Roles.ToList();
        _permissions = currentUser.Permissions.ToList();
    }
}
