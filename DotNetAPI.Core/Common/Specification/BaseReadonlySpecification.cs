namespace DotNetAPI.Core.Common.Specification;

public abstract class BaseReadonlySpecification<T> : BaseSpecification<T>, IReadonlySpecification<T>
{
    protected BaseReadonlySpecification() : base()
    {

    }

    public bool AsNoTracking { get; private set; }

    protected virtual void ApplyAsNoTracking()
    {
        AsNoTracking = true;
    }
}
