using Microsoft.EntityFrameworkCore.Query;
using DotNetAPI.Core.Common.Pagination;
using System.Linq.Expressions;

namespace DotNetAPI.Core.Common.Specification;

public abstract class BaseSpecification<T> : ISpecification<T>
{
    protected BaseSpecification()
    {

    }
    public WhereSpecification<T> Criteria { get; private set; } = default!;
    public List<Func<IQueryable<T>, IIncludableQueryable<T, object>>> Includes { get; } = new List<Func<IQueryable<T>, IIncludableQueryable<T, object>>>();
    public Expression<Func<T, object>> GroupBy { get; private set; } = null!;
    public Expression<Func<T, object>> Distinct { get; private set; } = null!;

    public string CacheKey { get; private set; } = default!;

    public bool AsSplitQuery { get; private set; } = false;

    public Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy { get; private set;} = null!;

    public SortParameters<T>? SortParameters { get; private set; }

    public bool UsePersistentCache { get; private set; } = false;

    protected virtual void AddInclude(Func<IQueryable<T>, IIncludableQueryable<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    protected virtual void ApplyOrderBy(Func<IQueryable<T>, IOrderedQueryable<T>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    protected virtual void ApplyOrderBy(SortParameters<T> sortParameters)
    {
        SortParameters = sortParameters;
    }

    protected virtual void ApplyGroupBy(Expression<Func<T, object>> groupByExpression)
    {
        GroupBy = groupByExpression;
    }

    protected virtual void ApplyDistinct(Expression<Func<T, object>> distinctExpression)
    {
        Distinct = distinctExpression;
    }

    protected virtual void ApplyCache(string specificationName, bool usePersistentCache, params object[] args)
    {
        CacheKey = $"{specificationName}-{string.Join("-", args)}";
        UsePersistentCache = usePersistentCache;
    }

    protected virtual void ApplyAsSplitQuery()
    {
        AsSplitQuery = true;
    }

    protected virtual void ApplyCriteria(WhereSpecification<T> criteria)
    {
        Criteria = criteria;
    }
}
