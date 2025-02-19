namespace DotNetAPI.Domain.Common.Interfaces;

public interface ICurrentUser
{
    public int Id { get; }

    public string UserName { get; }

    public string Email { get; }

    public string Type { get; }

    public bool IsVerified { get; }

    public bool IsAuthenticated { get; }

    bool IsUnauthorized(bool WebsiteIsLocked);

}
