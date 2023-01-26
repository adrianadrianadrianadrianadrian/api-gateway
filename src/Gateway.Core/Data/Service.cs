namespace Gateway.Core.Data;

using System.Collections.Generic;

public record Service(
    string Name,
    string Host,
    IEnumerable<Endpoint> Endpoints
);