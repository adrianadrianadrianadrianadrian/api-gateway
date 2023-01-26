namespace Gateway.Core.Mutators;

using System.Net.Http;
using Gateway.Core.Abstractions;
using Gateway.Core.Data;
using System;
using Bearded.Monads;

public class UrlMutator : IHttpRequestMutator
{
    public bool IsFor(Endpoint b) => true;

    public void Mutate(HttpRequestMessage request, Endpoint endpoint)
    {
        request.RequestUri = new Uri($"{endpoint.Host}{endpoint.OutboundRoute}{request.Query().ElseDefault()}");
    }
}