namespace Gateway.IO;

using System.Threading.Tasks;
using Gateway.Core.Abstractions;
using Gateway.Core.Data;
using Bearded.Monads;

public class CachedServiceReader : IServiceReader
{
    private readonly ICache<Service> _cache;
    private readonly IServiceReader _reader;

    public CachedServiceReader(
        ICache<Service> cache,
        IServiceReader serviceReader)
    {
        _cache = cache;
        _reader = serviceReader;
    }

    public Task<Option<Service>> Read(string serviceName)
        => _cache.Get(serviceName)
                 .Select(s => Task.FromResult(Option<Service>.Return(s)))
                 .Else(() => _reader.Read(serviceName).Select(mS =>
                 {
                     mS.Do(s => _cache.Set(serviceName, s));
                     return mS;
                 }));
}