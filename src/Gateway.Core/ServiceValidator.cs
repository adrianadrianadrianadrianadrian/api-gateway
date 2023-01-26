namespace Gateway.Core;

using Gateway.Core.Abstractions;
using Gateway.Core.Data;
using System.Linq;
using System;
using System.Collections.Generic;

public class ServiceValidator : IServiceValidator
{
    private readonly List<Func<Service, string?>> _checks = new List<Func<Service, string?>>();

    public ServiceValidator()
    {
        _checks.Add(DuplicateEndpoints);
    }

    public bool Validate(Service service, out string? error)
    {
        error = _checks.Select(c => c(service)).FirstOrDefault(err => err != null);
        return error == null;
    }

    private string? DuplicateEndpoints(Service service)
    {
        if (service.Endpoints.DistinctBy(e => e.InboundRoute).Count() != service.Endpoints.Count())
            return "Multiple endpoints with the same inbound route.";

        return null;
    }
}