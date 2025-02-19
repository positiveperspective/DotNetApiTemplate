using DotNetAPI.Core.Common.Repository;
using DotNetAPI.Domain.OrderDomain;

namespace DotNetAPI.Core.OrderUseCases.Repositories;

public interface IOrderRepository : IRepository<Order>
{
}
