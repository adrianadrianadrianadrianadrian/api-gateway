namespace Gateway.IO;

using System.Threading.Tasks;
using Gateway.Core.Abstractions;
using Gateway.Core.Data;
using Azure.Storage.Blobs;
using Newtonsoft.Json;
using System.IO;
using Bearded.Monads;
using Gateway.IO.Data;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System;

public class ServiceReader : IServiceReader
{
    private readonly BlobContainerClient _container;
    private readonly ILogger<ServiceReader> _logger;
    private readonly Dictionary<Policy, Type> _policies;
    private readonly JsonSerializer _serialiser;

    public ServiceReader(
        string connectionString,
        string container,
        ILogger<ServiceReader> logger,
        Dictionary<Policy, Type> policies)
    {
        _container = new BlobContainerClient(connectionString, container);
        _logger = logger;
        _policies = policies;
        _serialiser = new JsonSerializer();
        _serialiser.Converters.Add(new StringEnumConverter());
        _serialiser.Converters.Add(new PolicyConverter(_policies));
    }

    public async Task<Option<Service>> Read(string serviceName)
    {
        _logger.LogInformation("Reading service ({serviceName}) from storage", serviceName);

        try
        {
            var client = _container.GetBlobClient(serviceName);

            using (var stream = await client.OpenReadAsync())
            using (StreamReader reader = new StreamReader(stream))
            using (JsonTextReader jsonReader = new JsonTextReader(reader))
            {
                return _serialiser
                    .Deserialize<ServiceItem>(jsonReader)
                    .AsOption()
                    .Select(s => s!.ToCore());
            }
        }
        catch (Azure.RequestFailedException e) when (e.Status == 404)
        {
            return Option<Service>.None;
        }
    }
}