namespace Gateway.Core;

using Gateway.Core.Abstractions;
using Gateway.Core.Data;
using System.Threading.Tasks;

public class ServiceHandler
{
    private readonly IServiceWriter _writer;
    private readonly IServiceValidator _validator;

    public ServiceHandler(
        IServiceWriter writer,
        IServiceValidator validator)
    {
        _writer = writer;
        _validator = validator;
    }

    public async Task<string?> HandleUpsert(Service service)
    {
        if (!_validator.Validate(service, out var error))
            return error;

        await _writer.Write(service);
        return null;
    }
}