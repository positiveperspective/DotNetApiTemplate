using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Threading;

namespace DotNetAPI.Controllers;

[ApiController]
[Route("Orders")]
public class OrdersController : ApiController
{
    private readonly IConfiguration configuration;
    IHttpContextAccessor _accessor;
    HttpContext _context;
    IHostEnvironment _hostEnvironment;
    
    public OrdersController(IConfiguration _configuration, IHttpContextAccessor accessor, IHostEnvironment hostEnvironment)
    {
        configuration = _configuration;
        _accessor = accessor;
        _hostEnvironment = hostEnvironment;
        _context = _accessor.HttpContext;
    }

    [HttpGet]
    [Route("Cart", Name = nameof(Cart))]
    public async Task<ActionResult> Cart(string Language, CancellationToken cancellationToken)
    {
        return Ok();
    }
}