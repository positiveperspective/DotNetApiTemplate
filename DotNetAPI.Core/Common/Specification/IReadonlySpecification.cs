namespace DotNetAPI.Core.Common.Specification;

public interface IReadonlySpecification<T> : ISpecification<T>
{
    bool AsNoTracking { get; }
}
