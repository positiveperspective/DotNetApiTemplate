using Microsoft.EntityFrameworkCore;
using DotNetAPI.Core.Common.Specification;

namespace DotNetAPI.Infrastructure.Database.Repositories;

public class ReadonlySpecificationEvaluator<TEntity> : SpecificationEvaluator<TEntity> where TEntity : class
{
    public ReadonlySpecificationEvaluator()
    {

    }

    public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, IReadonlySpecification<TEntity> specification)
    {
        var query = ReadonlySpecificationEvaluator<TEntity>.GetQuery(inputQuery, specification);

        if(specification.AsNoTracking)
        {
            query = query.AsNoTracking();
        }

        return query;
    }
}
