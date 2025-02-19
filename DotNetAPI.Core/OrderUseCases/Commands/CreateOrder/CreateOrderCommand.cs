using DotNetAPI.Core.Common;
using DotNetAPI.Core.OrderUseCases.Dtos;
using DotNetAPI.Core.OrderUseCases.Repositories;
using DotNetAPI.Domain.OrderDomain;
using MediatR;

namespace DotNetAPI.Core.OrderUseCases.Commands.CreateOrder;

public class CreateOrderCommand : IRequest
{
    public OrderDto Dto { get; private set; }

    public CreateOrderCommand(OrderDto dto)
    {
        Dto = dto;
    }

    internal class Handler : IRequestHandler<CreateOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        public Handler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            Order order = new Order(request.Dto.OrderDate, request.Dto.OrderCustomerID, request.Dto.EventId, request.Dto.RequestQty, request.Dto.ServiceDate1, request.Dto.ServiceDate2, request.Dto.ServiceDate3, request.Dto.OrderBondIsPaid, request.Dto.OrderBondPaidDate, request.Dto.OrderDifferenceIsPaid, request.Dto.OrderDifferenceDate, request.Dto.OrderPriority, request.Dto.OrderIsDispatched, request.Dto.OrderShippedDate, request.Dto.OrderProgress, request.Dto.OrderDeliveredDate, request.Dto.StoreID, request.Dto.StaffID, request.Dto.PayUOrderID, request.Dto.PayPalOrderID);

            await _orderRepository.Add(order, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

        }

    }
}
