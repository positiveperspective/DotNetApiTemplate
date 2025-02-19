namespace DotNetAPI.Infrastructure.Database.Repositories;

public class SpecificationLocalCache
{
    private IList<string> _specificationCacheKeys = new List<string>();

    public IReadOnlyList<string> SpecificationCacheKeys => _specificationCacheKeys.ToList();

    public void Add(string? specificationCacheKey)
    {
        if(specificationCacheKey != null)
        {
            _specificationCacheKeys.Add(specificationCacheKey);
        }
    }

    public bool HasKey(string key)
    {
        return _specificationCacheKeys.Any(k => k == key);
    }
}
