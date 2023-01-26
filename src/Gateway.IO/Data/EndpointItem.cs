namespace Gateway.IO.Data;

using Gateway.Core.Data;
using System.Net.Http;
using System.Collections.Generic;

public record EndpointItem(
    string InboundRoute,
    string OutboundRoute,
    IEnumerable<IPolicy> Policies,
    string Method
)
{
    public Endpoint ToCore(string serviceHostName)
        => new(
            serviceHostName,
            InboundRoute,
            OutboundRoute,
            Policies,
            new HttpMethod(Method)
        );

    public static EndpointItem FromCore(Endpoint endpoint)
        => new(
            endpoint.InboundRoute,
            endpoint.OutboundRoute,
            endpoint.Policies,
            endpoint.Method.ToString()
        );
}