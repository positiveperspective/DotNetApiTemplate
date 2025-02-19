using DotNetAPI.Core.OrderUseCases.Dtos;
using DotNetAPI.Domain.OrderDomain;

namespace DotNetAPI.Core.OrderUseCases;

internal class MappingProfile : AutoMapper.Profile
{
    public MappingProfile()
    {
        CreateMap<Order, OrderDto>();
    }
}
