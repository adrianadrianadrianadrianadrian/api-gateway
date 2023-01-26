namespace Gateway.Core.Abstractions;

using Bearded.Monads;
using System.Net.Http.Headers;

public interface IJwtValidator<C>
{
    Option<C> Validate(HttpRequestHeaders headers);
}