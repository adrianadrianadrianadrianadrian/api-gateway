namespace Gateway.Core.Abstractions;

using System.Threading.Tasks;
using Gateway.Core.Data;

public interface IServiceWriter
{
    Task Write(Service service);
}