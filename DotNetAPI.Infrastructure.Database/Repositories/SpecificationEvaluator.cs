using Microsoft.EntityFrameworkCore;
using DotNetAPI.Core.Common.Constants;
using DotNetAPI.Core.Common.Specification;

namespace DotNetAPI.Infrastructure.Database.Repositories;

public class SpecificationEvaluator<TEntity> where TEntity : class
{
    public SpecificationEvaluator()
    {

    }

    public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
    {
        var query = inputQuery;

        query = specification.Includes.Aggregate(query, (current, include) => include(current));

        if(specification.Criteria != null)
        {
            query = query.Where(specification.Criteria.ToExpression());
        }

        if(specification.Distinct != null)
        {
            query = query.DistinctBy(specification.Distinct);
        }

        if(specification.GroupBy != null)
        {
            query = query.GroupBy(specification.GroupBy).SelectMany(x => x);
        }

        if(specification.OrderBy != null)
        {
            query = specification.OrderBy(query);
        }

        if(specification.SortParameters != null)
        {
            query = specification.SortParameters.SortDirection == SortDirectionConstants.Descending ?
                query.OrderByDescending(specification.SortParameters.SortBy) :
                query.OrderBy(specification.SortParameters.SortBy);
        }

        if(specification.AsSplitQuery)
        {
            query = query.AsSplitQuery();
        }

        query = query.TagWith(specification.GetType().Name);

        return query;
    }
}
