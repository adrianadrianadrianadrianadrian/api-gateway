namespace Gateway.Core.Data;

using System.Net.Http;
using System.Collections.Generic;

public record Endpoint(
    string Host,
    string InboundRoute,
    string OutboundRoute,
    IEnumerable<IPolicy> Policies,
    HttpMethod Method
);