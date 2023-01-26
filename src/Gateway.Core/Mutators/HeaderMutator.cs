namespace Gateway.Core.Mutators;

using System.Net.Http;
using Gateway.Core.Abstractions;
using Gateway.Core.Data;
using Bearded.Monads;
using System.Linq;

public class HeaderMutator : IHttpRequestMutator
{
    public bool IsFor(Endpoint b) => b.Policies.Any(p => p is AddHeaderPolicy);

    public void Mutate(HttpRequestMessage request, Endpoint endpoint)
        => endpoint.Policies
                   .FirstOrNone(p => p is AddHeaderPolicy)
                   .SelectMany(p => p.MaybeCast<AddHeaderPolicy>())
                   .Do(p =>
                   {
                       foreach (var (k, v) in p.Headers)
                       {
                           if (!request.Headers.TryAddWithoutValidation(k, v) && request.Content != null)
                               request.Content.Headers.TryAddWithoutValidation(k, v);
                       }
                   });
}