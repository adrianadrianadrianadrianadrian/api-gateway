namespace Gateway.Core.Abstractions;

using System.Net.Http;
using Gateway.Core.Data;

public interface IHttpRequestMutator : IsFor<Endpoint>
{
    void Mutate(HttpRequestMessage request, Endpoint endpoint);
}