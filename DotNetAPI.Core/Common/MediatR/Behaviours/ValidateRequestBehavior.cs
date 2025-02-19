using MediatR;
using DotNetAPI.Core.Common.MediatR.Policies;

namespace DotNetAPI.Core.Common.MediatR.Behaviours;

internal class ValidateRequestBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IValidationPolicy<TRequest> _validationPolicy = null!;

    public ValidateRequestBehavior()
    {

    }

    public ValidateRequestBehavior(IValidationPolicy<TRequest> validationPolicy)
    {
        _validationPolicy = validationPolicy;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if(_validationPolicy != null)
        {
            await _validationPolicy.ValidateAsync(request, cancellationToken);
        }

        return await next();
    }
}
