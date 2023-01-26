namespace Gateway.Host.Controllers;

using Microsoft.AspNetCore.Mvc;
using Gateway.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Gateway.IO.Data;

[Route("service")]
public class ServiceController : ControllerBase
{
    private readonly ServiceHandler _handler;
    private readonly ILogger<ServiceController> _logger;

    public ServiceController(
        ServiceHandler handler,
        ILogger<ServiceController> logger)
    {
        _handler = handler;
        _logger = logger;
    }

    [HttpPost]
    [Route("upsert")]
    public async Task<ActionResult> UpsertService([FromBody] ServiceItem serviceToUpsert)
    {
        var error = await _handler.HandleUpsert(serviceToUpsert.ToCore());

        if (error != null)
            return new ContentResult { StatusCode = 400, Content = error };

        return new OkResult();
    }
}