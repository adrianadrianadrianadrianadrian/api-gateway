namespace Gateway.Tests.Unit;

using NUnit.Framework;
using AutoFixture;
using System.Net.Http;
using System;
using static Gateway.Core.HttpRequestHelpers;

public class HttpRequestHelperTests
{
    private readonly Fixture _fixture;

    public HttpRequestHelperTests()
    {
        _fixture = new Fixture();
    }

    [Test]
    public void ServiceName_CorrectlyExtracts_Name()
    {
        var serviceName = _fixture.Create<string>();
        var method = _fixture.Create<HttpMethod>();
        var request = new HttpRequestMessage(method, new Uri($"http://localhost.com:123/{serviceName}/someotherpath/ok?test=2"));

        var name = request.ServiceName();

        Assert.True(name.IsSome);
        Assert.That(serviceName, Is.EqualTo(name.ForceValue()));
    }

    [Test]
    public void InboundRoute_CorrectlyExtracts_RouteOnly()
    {
        var route = "/a/b/c/d/e";
        var method = _fixture.Create<HttpMethod>();
        var request = new HttpRequestMessage(method, new Uri($"http://localhost.com{route}?test=2"));

        var r = request.InboundRoute();

        Assert.True(r.IsSome);
        Assert.That(route, Is.EqualTo(r.ForceValue()));
    }

    [Test]
    public void Query_CorrectlyExtracts_Query()
    {
        var route = "/a/b/c/d/e";
        var query = "?you=me&me=you";
        var method = _fixture.Create<HttpMethod>();
        var request = new HttpRequestMessage(method, new Uri($"http://localhost.com{route}{query}"));

        var q = request.Query();

        Assert.True(q.IsSome);
        Assert.That(query, Is.EqualTo(q.ForceValue()));
    }
}