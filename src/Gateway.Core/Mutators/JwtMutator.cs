namespace Gateway.Core.Mutators;

using System.Net.Http;
using Gateway.Core.Abstractions;
using Gateway.Core.Data;
using Gateway.Contracts;
using System.Linq;
using Newtonsoft.Json;
using Bearded.Monads;

public class JwtMutator : IHttpRequestMutator
{
    private readonly IJwtValidator<AuthenticatedUser> _jwtValidator;

    public JwtMutator(IJwtValidator<AuthenticatedUser> jwtValidator)
    {
        _jwtValidator = jwtValidator;
    }

    public bool IsFor(Endpoint e) => e.Policies.Any(p => p is JwtValidationPolicy);

    public void Mutate(HttpRequestMessage request, Endpoint endpoint)
    {
        var user = _jwtValidator.Validate(request.Headers).ElseThrow(GatewayException.JwtValidationFailed);

        endpoint.Policies
                .FirstOrNone(p => p is JwtValidationPolicy)
                .SelectMany(p => p.MaybeCast<JwtValidationPolicy>())
                .Do(p => request.Headers.Add(p.ClaimsHeaderName, JsonConvert.SerializeObject(user)));
    }
}