using Microsoft.EntityFrameworkCore.Query;
using DotNetAPI.Core.Common.Pagination;
using System.Linq.Expressions;

namespace DotNetAPI.Core.Common.Specification;

public interface ISpecification<T>
{
    WhereSpecification<T> Criteria { get; }
    Expression<Func<T, object>> GroupBy { get; }
    Expression<Func<T, object>> Distinct { get; }
    Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy { get; }
    List<Func<IQueryable<T>, IIncludableQueryable<T, object>>> Includes { get; }

    string CacheKey { get; }

    bool AsSplitQuery { get; }

    SortParameters<T>? SortParameters { get; }

    bool UsePersistentCache { get; }
}
