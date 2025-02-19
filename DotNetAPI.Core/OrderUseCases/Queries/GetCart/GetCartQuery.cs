using AutoMapper;
using DotNetAPI.Core.OrderUseCases.Dtos;
using DotNetAPI.Core.OrderUseCases.Repositories;
using DotNetAPI.Core.OrderUseCases.Specifications;
using DotNetAPI.Domain.OrderDomain;
using MediatR;

namespace DotNetAPI.Core.OrderUseCases.Queries.GetCart;

public class GetCartQuery : IRequest<IEnumerable<OrderDto>>
{
    public GetCartQuery()
    {

    }

    internal class Handler : IRequestHandler<GetCartQuery, IEnumerable<OrderDto>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        public Handler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDto>> Handle(GetCartQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Order> orders = await _orderRepository.List(new OrderSpecification(), cancellationToken);

            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }
    }
}
