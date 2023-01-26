namespace Gateway.IO;

using Bearded.Monads;

public record JwtToken(string Header, string Payload, string Signature)
{
    public static Option<JwtToken> Parse(string token)
        => token.Split(".") switch
        {
            [var header, var payload, var sig] => new JwtToken(header, payload, sig),
            _ => Option<JwtToken>.None
        };
}