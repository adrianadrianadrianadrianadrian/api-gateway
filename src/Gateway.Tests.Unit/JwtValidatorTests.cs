namespace Gateway.Tests.Unit;

using NUnit.Framework;
using Gateway.IO;
using System.Net.Http;

public class JwtValidatorTests
{
    private readonly string _secret;

    public JwtValidatorTests()
    {
        _secret = "c2VjcmV0YnJvMQ==";
    }

    [Test]
    public void HappyPath()
    {
        var validator = new JwtValidator(_secret);
        var req = new HttpRequestMessage();
        var jwt = "eyJhbGciOiJIUzI1NiJ9.eyJVc2VySWQiOiJBZHJpYW4xMjMifQ.iAceIqrKusDlCKHUyazXduKG5h8E4mxlguyq0ilz5AE";

        req.Headers.Add("Cookie", $"jwt={jwt};");

        var maybeUser = validator.Validate(req.Headers);
        Assert.True(maybeUser.IsSome);
        Assert.That(maybeUser.ForceValue().UserId, Is.EqualTo("Adrian123"));
    }
}