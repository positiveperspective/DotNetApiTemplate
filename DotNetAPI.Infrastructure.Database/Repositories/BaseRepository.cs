using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using DotNetAPI.Core.Common.Cache;
using DotNetAPI.Core.Common.Repository;
using DotNetAPI.Core.Common.Specification;
using DotNetAPI.Infrastructure.Database.Repositories;

namespace DotNetAPI.Infrastructure.Database.Repositories;

public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity: class
{
    private readonly DotNetAPIContext _context;
    private readonly SpecificationLocalCache _specificationLocalCache;
    private readonly IMemoryCache _memoryCache;

    private string GetKeyForMethod(string inputCacheKey, string method) => $"{inputCacheKey}-{method}";

    private string _first => "First";

    private string _firstOrDefault => "FirstOrDefault";

    private string _list => "List";

    public BaseRepository(DotNetAPIContext context, SpecificationLocalCache specificationLocalCache, IMemoryCache memoryCache)
    {
        _context = context;
        _specificationLocalCache = specificationLocalCache;
        _memoryCache = memoryCache;
    }

    public virtual async Task Add(TEntity entity, CancellationToken cancellationToken)
    {
        await _context.Set<TEntity>().AddAsync(entity, cancellationToken);
    }

    public virtual async Task AddRange(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        await _context.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
    }

    public virtual async Task<IReadOnlyList<TEntity>> List(ISpecification<TEntity> specification, CancellationToken cancellationToken)
    {
        string cacheKey = GetKeyForMethod(specification.CacheKey, _list);

        if(specification.UsePersistentCache)
        {
            return await GetAsync(cacheKey, async () =>
            {
                return await Task.Run(() => GetSpecification(specification, cacheKey).ToList(), cancellationToken);
            });
        }

        return await Task.Run(() => GetSpecification(specification, cacheKey).ToList(), cancellationToken);
    }

    public virtual async Task<TEntity?> FirstOrDefault(ISpecification<TEntity> specification, CancellationToken cancellationToken)
    {
        string cacheKey = GetKeyForMethod(specification.CacheKey, _firstOrDefault);

        if (specification.UsePersistentCache)
        {
            return await GetAsync(cacheKey, async () =>
            {
                return await Task.Run(() => GetSpecification(specification, cacheKey).FirstOrDefault(), cancellationToken);
            });
        }

        return await Task.Run(() => GetSpecification(specification, cacheKey).FirstOrDefault(), cancellationToken);
    }

    public virtual async Task<TEntity> First(ISpecification<TEntity> specification, CancellationToken cancellationToken)
    {
        string cacheKey = GetKeyForMethod(specification.CacheKey, _first);

        if (specification.UsePersistentCache)
        {
            return await GetAsync(cacheKey, async () =>
            {
                return await Task.Run(() => GetSpecification(specification, cacheKey).First(), cancellationToken);
            });
        }

        return await Task.Run(() => GetSpecification(specification, cacheKey).First(), cancellationToken);
    }

    public virtual async Task Remove(TEntity entity, CancellationToken cancellationToken)
    {
        await Task.Run(() => _context.Set<TEntity>().Remove(entity), cancellationToken);
    }

    public virtual async Task RemoveRange(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        await Task.Run(() => _context.Set<TEntity>().RemoveRange(entities), cancellationToken);
    }

    public virtual async Task Update(TEntity entity, CancellationToken cancellationToken)
    {
        await Task.Run(() => _context.Update(entity), cancellationToken);
    }

    public virtual async Task<IReadOnlyList<TEntity>> List(IReadonlySpecification<TEntity> specification, CancellationToken cancellationToken)
    {
        return await Task.Run(() => GetReadonlySpecification(specification).ToList(), cancellationToken);
    }

    public virtual async Task<TEntity> First(IReadonlySpecification<TEntity> specification, CancellationToken cancellationToken)
    {
        return await Task.Run(() => GetReadonlySpecification(specification).First(), cancellationToken);
    }

    private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> spec)
    {
        return SpecificationEvaluator<TEntity>.GetQuery(_context.Set<TEntity>().AsQueryable(), spec);
    }

    private IQueryable<TEntity> ApplyLocalSpecification(ISpecification<TEntity> spec)
    {
        return SpecificationEvaluator<TEntity>.GetQuery(_context.Set<TEntity>().Local.AsQueryable(), spec);
    }

    private IQueryable<TEntity> ApplyReadonlySpecification(IReadonlySpecification<TEntity> spec)
    {
        return ReadonlySpecificationEvaluator<TEntity>.GetQuery(_context.Set<TEntity>().AsQueryable(), spec);
    }

    private IQueryable<TEntity> GetSpecification(ISpecification<TEntity> spec, string cacheKey)
    {
        if (_specificationLocalCache.HasKey(cacheKey))
        {
            return ApplyLocalSpecification(spec);
        }
        else
        {
            CreateKey(cacheKey);
            return ApplySpecification(spec);
        }
    }

    private IQueryable<TEntity> GetReadonlySpecification(IReadonlySpecification<TEntity> spec)
    {
        return ApplyReadonlySpecification(spec);
    }

    private void CreateKey(string key)
    {
        _specificationLocalCache.Add(key);
    }

    public async Task<bool> Any(ISpecification<TEntity> specification, CancellationToken cancellationToken)
    {
        if (specification.UsePersistentCache)
        {
            return await GetAsync(specification.CacheKey, async () =>
            {
                return await Task.Run(() => ApplySpecification(specification).AnyAsync(), cancellationToken);
            });
        }
        return await Task.Run(() => ApplySpecification(specification).AnyAsync(), cancellationToken);
    }

    private Task<TResult> GetAsync<TResult>(string key, Func<Task<TResult>> factory)
    {
        return _memoryCache.GetOrCreateAsync(key, cacheEntry =>
        {
            InMemoryCache.ConfigsCacheToken.Apply(cacheEntry);

            return factory();
        });
    }

    public async Task<int> Count(ISpecification<TEntity> specification, CancellationToken cancellationToken)
    {
        if (specification.UsePersistentCache)
        {
            return await GetAsync(specification.CacheKey, async () =>
            {
                return await Task.Run(() => ApplySpecification(specification).CountAsync(), cancellationToken);
            });
        }
        return await Task.Run(() => ApplySpecification(specification).CountAsync(), cancellationToken);
    }
}
