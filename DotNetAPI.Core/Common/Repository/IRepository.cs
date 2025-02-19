using DotNetAPI.Core.Common.Specification;

namespace DotNetAPI.Core.Common.Repository;

public interface IRepository<TEntity>
{
    public Task Add(TEntity entity, CancellationToken cancellationToken);

    public Task AddRange(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

    public Task<IReadOnlyList<TEntity>> List(ISpecification<TEntity> specification, CancellationToken cancellationToken);

    public Task<TEntity?> FirstOrDefault(ISpecification<TEntity> specification, CancellationToken cancellationToken);

    public Task<TEntity> First(ISpecification<TEntity> specification, CancellationToken cancellationToken);

    public Task Remove(TEntity entity, CancellationToken cancellationToken);

    public Task RemoveRange(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

    public Task Update(TEntity entity, CancellationToken cancellationToken);

    public Task<IReadOnlyList<TEntity>> List(IReadonlySpecification<TEntity> specification, CancellationToken cancellationToken);

    public Task<TEntity> First(IReadonlySpecification<TEntity> specification, CancellationToken cancellationToken);

    public Task<bool> Any(ISpecification<TEntity> specification, CancellationToken cancellationToken);

    public Task<int> Count(ISpecification<TEntity> specification, CancellationToken cancellationToken);


}
