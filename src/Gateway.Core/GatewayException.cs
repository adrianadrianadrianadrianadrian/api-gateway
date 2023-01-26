namespace Gateway.Core.Data;

using System;
using System.Net.Http;
using System.Net;

public class GatewayException : Exception
{
    public HttpResponseMessage ErrorResponse { get; init; }

    public GatewayException(HttpStatusCode statusCode) : base()
    {
        ErrorResponse = new HttpResponseMessage(statusCode);
    }

    public static GatewayException JwtValidationFailed() => new GatewayException(HttpStatusCode.Forbidden);
}