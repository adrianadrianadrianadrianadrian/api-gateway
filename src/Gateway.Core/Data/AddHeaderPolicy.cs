namespace Gateway.Core.Data;

using System.Collections.Generic;

public record AddHeaderPolicy(Dictionary<string, string> Headers) : IPolicy
{
    public Policy Policy => Policy.AddHeader;
}