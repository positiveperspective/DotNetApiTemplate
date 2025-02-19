using MediatR;
using DotNetAPI.Core.Common.Exceptions;
using DotNetAPI.Core.Common.MediatR.Policies;

namespace DotNetAPI.Core.Common.MediatR.Behaviours;

internal class AuthorizeRequestBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IAuthorizationPolicy<TRequest> _authorizationPolicy = null!;

    public AuthorizeRequestBehavior()
    {

    }

    public AuthorizeRequestBehavior(IAuthorizationPolicy<TRequest> authorizationPolicy)
    {
        _authorizationPolicy = authorizationPolicy;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if(_authorizationPolicy != null)
        {
            bool isAuthorized = await _authorizationPolicy.AuthorizeAsync(request, cancellationToken);
            if (!isAuthorized)
            {
                throw new AccessForbiddenException();
            }
        }

        return await next();
    }
}