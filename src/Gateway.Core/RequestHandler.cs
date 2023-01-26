namespace Gateway.Core;

using System.Net.Http;
using System.Threading.Tasks;
using Gateway.Core.Abstractions;
using System.Collections.Generic;
using System.Linq;
using Bearded.Monads;
using Gateway.Core.Data;
using System;
using System.Net;

public class RequestHandler
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IEnumerable<IHttpRequestMutator> _mutators;
    private readonly IServiceReader _serviceReader;

    public RequestHandler(
        IHttpClientFactory httpClientFactory,
        IEnumerable<IHttpRequestMutator> mutators,
        IServiceReader serviceReader
    )
    {
        _httpClientFactory = httpClientFactory;
        _mutators = mutators;
        _serviceReader = serviceReader;
    }

    public async Task<HttpResponseMessage> Handle(HttpRequestMessage request)
    {
        try
        {
            var response = await request
                .ServiceName()
                .And(request.InboundRoute())
                .Then((name, route) => new { Name = name, Route = route })
                .SelectMany(pair => _serviceReader.Read(pair.Name).Select(MaybeEndpoint(pair.Route)))
                .SelectMany(mEndpoint => mEndpoint.SelectMany<Endpoint, HttpResponseMessage>(async endpoint =>
                {
                    foreach (var m in _mutators.Where(m => m.IsFor(endpoint)))
                        m.Mutate(request, endpoint);

                    using (var client = _httpClientFactory.CreateClient())
                        return await client.SendAsync(request);
                }));

            return response.Else(() => new HttpResponseMessage(HttpStatusCode.NotFound));
        }
        catch (GatewayException e)
        {
            return e.ErrorResponse;
        }
    }

    private Func<Option<Service>, Option<Endpoint>> MaybeEndpoint(string route) =>
        mS => mS.SelectMany(s => s.Endpoints.FirstOrNone(e => $"/{s.Name}{e.InboundRoute}" == route));
}