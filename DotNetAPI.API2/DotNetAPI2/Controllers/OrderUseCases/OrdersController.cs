using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Threading;
using DotNetAPI.Core.OrderUseCases.Dtos;
using Newtonsoft.Json.Linq;
using DotNetAPI.Core.OrderUseCases.Commands.CreateOrder;
using DotNetAPI.Core.OrderUseCases.Queries.GetCart;

namespace DotNetAPI.Controllers;

[ApiController]
[Route("Orders")]
public class OrdersController : ApiController
{
    
    public OrdersController()
    {
    }

    [HttpPost]
    [Route("", Name = nameof(CreateOrder))]
    public async Task<ActionResult> CreateOrder([FromBody] OrderDto dto, CancellationToken cancellationToken)
    {
        await Mediator.Send(new CreateOrderCommand(dto), cancellationToken);

        return Ok();
    }

    [HttpGet]
    [Route("Cart", Name = nameof(Cart))]
    public async Task<ActionResult> Cart(CancellationToken cancellationToken)
    {
        GetCartQuery query = new GetCartQuery();

        IEnumerable<OrderDto> dto = await Mediator.Send(query, cancellationToken);

        return Ok(dto);
    }
}