namespace Gateway.IO;

using Gateway.Core.Abstractions;
using Gateway.Contracts;
using Bearded.Monads;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Cryptography;
using System;
using System.Text;
using Newtonsoft.Json;
using Microsoft.AspNetCore.WebUtilities;

public class JwtValidator : IJwtValidator<AuthenticatedUser>
{
    private readonly string _jwtSecret;
    private readonly string _jwtCookieName = "jwt";

    public JwtValidator(string jwtSecret)
    {
        _jwtSecret = jwtSecret;
    }

    public Option<AuthenticatedUser> Validate(HttpRequestHeaders headers)
        => from token in headers.GetCookies(_jwtCookieName).FirstOrNone()
           from jwt in JwtToken.Parse(token[_jwtCookieName].Value)
           from payload in VerifyJwt(jwt, _jwtSecret)
           from user in MaybeUser(payload)
           select user;

    public Option<string> VerifyJwt(JwtToken token, string secret)
    {
        var hasher = new HMACSHA256(Convert.FromBase64String(secret));
        var toSign = Encoding.UTF8.GetBytes($"{token.Header}.{token.Payload}");
        var signature = hasher.ComputeHash(toSign);

        if (WebEncoders.Base64UrlEncode(signature) == token.Signature)
            return token.Payload;

        return Option<string>.None;
    }

    public Option<AuthenticatedUser> MaybeUser(string payload)
    {
        var raw = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(payload));
        try
        {
            return JsonConvert
                .DeserializeObject<AuthenticatedUser>(raw)
                .AsOption()
                .Select(a => a!);
        }
        catch { }

        return Option<AuthenticatedUser>.None;
    }
}