using DotNetAPI.Core.Common.Specification;
using DotNetAPI.Domain.OrderDomain;

namespace DotNetAPI.Core.OrderUseCases.Specifications;

public class OrderSpecification : BaseSpecification<Order>
{
    public OrderSpecification(bool usePersistentCache = false) : base()
    {
        ApplyCache(nameof(OrderSpecification), usePersistentCache, "all-data");
    }
}
