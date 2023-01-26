namespace Gateway.Core.Data;

public enum Policy
{
    JwtValidation,
    AddHeader
}

public interface IPolicy
{
    Policy Policy { get; }
}