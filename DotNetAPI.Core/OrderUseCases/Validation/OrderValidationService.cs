using DotNetAPI.Core.OrderUseCases.Repositories;
using DotNetAPI.Domain.Common.Interfaces;

namespace DotNetAPI.Core.OrderUseCases.Validation
{
    public class OrderValidationService : IOrderValidationService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IDateTime _dateTime;

        public OrderValidationService(IOrderRepository orderRepository, ICurrentUser currentUser, IDateTime dateTime)
        {
            _orderRepository = orderRepository;
            _currentUser = currentUser;
            _dateTime = dateTime;
        }

        //public Task ValidateSomething() { }
    }
}
