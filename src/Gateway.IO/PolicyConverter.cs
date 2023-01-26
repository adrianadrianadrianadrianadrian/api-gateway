namespace Gateway.IO;

using Newtonsoft.Json;
using System;
using Gateway.Core.Data;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;

public class PolicyConverter : JsonConverter
{
    private readonly Dictionary<Policy, Type> _policies;

    public PolicyConverter(Dictionary<Policy, Type> policies)
    {
        _policies = policies;
    }

    public override bool CanConvert(Type objectType) => typeof(IPolicy) == objectType;

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        => serializer.Serialize(writer, value);

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var obj = JObject.Load(reader);

        using (var memStream = new MemoryStream(Encoding.UTF8.GetBytes(obj.ToString())))
        using (var streamReader = new StreamReader(memStream))
        {
            var policy = obj[typeof(Policy).Name];
            if (policy != null)
            {
                var sPolicy = policy.Value<string>();
                if (Enum.TryParse<Policy>(sPolicy, true, out var ePolicy))
                {
                    var type = _policies[ePolicy];
                    return serializer.Deserialize(streamReader, type);
                }
            }
        }

        return null;
    }
}