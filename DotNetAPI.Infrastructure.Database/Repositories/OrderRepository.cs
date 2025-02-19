using Microsoft.Extensions.Caching.Memory;
using DotNetAPI.Core.OrderUseCases.Repositories;
using DotNetAPI.Domain.OrderDomain;

namespace DotNetAPI.Infrastructure.Database.Repositories;

internal class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    private readonly DotNetAPIContext _context;

    public OrderRepository(DotNetAPIContext context, SpecificationLocalCache specificationLocalCache, IMemoryCache memoryCache) : base(context, specificationLocalCache, memoryCache)
    {
        _context = context;
    }
}
