namespace Gateway.Host.Controllers;

using Microsoft.AspNetCore.Mvc;
using Gateway.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

[Route(Constants.IngressRoute)]
public class IngressController : ControllerBase
{
    private readonly RequestHandler _handler;
    private readonly ILogger<IngressController> _logger;

    public IngressController(
        RequestHandler handler,
        ILogger<IngressController> logger)
    {
        _handler = handler;
        _logger = logger;
    }

    [Route("{*chomp}")]
    public async Task Ingress()
    {
        _logger.LogDebug("Handling request. Path = {path}", Request.Path.Value);

        Request.RemovePathPrefix(Constants.IngressRoute);
        var response = await _handler.Handle(Request.HttpRequestMessage());
        await Response.Enrich(response);
    }
}