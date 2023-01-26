namespace Gateway.Tests.Unit;

using NUnit.Framework;
using Gateway.Core.Data;
using Gateway.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Converters;
using AutoFixture;

public class PolicyConverterTests
{
    private readonly Fixture _autoFixture;

    public PolicyConverterTests()
    {
        _autoFixture = new Fixture();
    }

    [Test]
    public void CanDeserialise()
    {
        var policies = new Dictionary<Policy, Type> {
            { Policy.AddHeader, typeof(AddHeaderPolicy) },
            { Policy.JwtValidation, typeof(JwtValidationPolicy) }
        };

        var secret = _autoFixture.Create<string>();
        var claimsHeader = _autoFixture.Create<string>();

        var jwtPolicy = "{\"Policy\":\"JwtValidation\",\"JwtSecret\":\"" + secret + "\",\"ClaimsHeaderName\":\"" + claimsHeader + "\"}";
        var policy = JsonConvert.DeserializeObject<IPolicy>(jwtPolicy, new PolicyConverter(policies), new StringEnumConverter());

        Assert.True(policy is JwtValidationPolicy);
        Assert.True(((JwtValidationPolicy)policy!).JwtSecret == secret);
        Assert.True(((JwtValidationPolicy)policy!).ClaimsHeaderName == claimsHeader);
    }
}