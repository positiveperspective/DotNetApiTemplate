namespace DotNetAPI.Core.Common.MediatR.Policies;

public interface IValidationPolicy<TRequest>
{
    Task ValidateAsync(TRequest request, CancellationToken cancellationToken);
}
