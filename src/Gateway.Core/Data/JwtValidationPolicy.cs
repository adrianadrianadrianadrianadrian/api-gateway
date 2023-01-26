namespace Gateway.Core.Data;

public record JwtValidationPolicy(string JwtSecret, string ClaimsHeaderName) : IPolicy
{
    public Policy Policy => Policy.JwtValidation;
}