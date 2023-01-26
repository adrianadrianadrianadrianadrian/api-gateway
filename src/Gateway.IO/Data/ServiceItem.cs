namespace Gateway.IO.Data;

using System.Collections.Generic;
using Gateway.Core.Data;
using System.Linq;

public record ServiceItem(
    string Name,
    string Host,
    IEnumerable<EndpointItem> Endpoints
)
{
    public Service ToCore()
        => new(
            Name,
            Host,
            Endpoints.Select(e => e.ToCore(Host))
        );

    public static ServiceItem FromCore(Service service)
        => new(
            service.Name,
            service.Host,
            service.Endpoints.Select(EndpointItem.FromCore)
        );
}