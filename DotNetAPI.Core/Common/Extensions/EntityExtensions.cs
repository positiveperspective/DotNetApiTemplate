using DotNetAPI.Core.Common.Exceptions;

namespace DotNetAPI.Core.Common.Extensions;

public static class EntityExtensions
{
    public static TEntity EnsureExists<TEntity>(this TEntity entity) where TEntity : class
    {
        if (entity == null)
        {
            throw new NotFoundException($"{typeof(TEntity).Name.Format()}_NOT_FOUND");
        }

        return entity;
    }

    public static bool Exists<TEntity>(this TEntity entity) where TEntity : class
    {
        return entity != null;
    }
}
