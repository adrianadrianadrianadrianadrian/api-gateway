namespace Gateway.IO;

using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Gateway.Core.Abstractions;
using Gateway.Core.Data;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using System;
using Gateway.IO.Data;

public class ServiceWriter : IServiceWriter
{
    private readonly BlobContainerClient _container;
    private readonly ILogger<ServiceWriter> _logger;

    public ServiceWriter(
        string connectionString,
        string container,
        ILogger<ServiceWriter> logger)
    {
        _container = new BlobContainerClient(connectionString, container);
        _logger = logger;
    }

    public async Task Write(Service service)
    {
        _logger.LogInformation("Writing service ({serviceName}) to storage.", service.Name);

        var client = _container.GetBlobClient(service.Name);
        var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ServiceItem.FromCore(service)));
        await client.UploadAsync(new BinaryData(bytes), true);
    }
}