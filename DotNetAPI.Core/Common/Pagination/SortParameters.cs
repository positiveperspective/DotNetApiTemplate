using System.Linq.Expressions;

namespace DotNetAPI.Core.Common.Pagination;

public class SortParameters<T>
{
    public Expression<Func<T, dynamic>> SortBy { get; init; }

    public string SortDirection { get; init; }

    public SortParameters(Expression<Func<T, dynamic>> sortBy, string sortDirection)
    {
        SortBy = sortBy;
        SortDirection = sortDirection;
    }

    public static SortParameters<T>? Create(Expression<Func<T, dynamic>>? sortBy, string? sortDirection)
    {
        if(sortBy == null || sortDirection == null)
        {
            return null;
        }

        return new SortParameters<T>(sortBy, sortDirection);
    }
}
