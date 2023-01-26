namespace Gateway.Core.Abstractions;

using System.Threading.Tasks;
using Gateway.Core.Data;
using Bearded.Monads;

public interface IServiceReader
{
    Task<Option<Service>> Read(string serviceName);
}