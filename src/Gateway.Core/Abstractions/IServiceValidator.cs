namespace Gateway.Core.Abstractions;

using Gateway.Core.Data;

public interface IServiceValidator
{
    bool Validate(Service service, out string? error);
}