namespace DotNetAPI.Core.Common.MediatR.Policies;

public interface IAuthorizationPolicy<TRequest>
{
    Task<bool> AuthorizeAsync(TRequest request, CancellationToken cancellationToken);
}
