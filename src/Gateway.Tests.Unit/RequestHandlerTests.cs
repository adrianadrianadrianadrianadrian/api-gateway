namespace Gateway.Tests.Unit;

using NUnit.Framework;
using NSubstitute;
using Gateway.Core;
using Gateway.Core.Abstractions;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;

public class RequestHandlerTests
{
    [Test]
    public async Task RequestHandler_Catches_GatewayExceptions()
    {
        var reader = Substitute.For<IServiceReader>();
        var httpClient = Substitute.For<IHttpClientFactory>();
        var handler = new RequestHandler(httpClient, new List<IHttpRequestMutator>(), reader);

        var req = new HttpRequestMessage();
        var res = await handler.Handle(req);

        Assert.That(res.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}