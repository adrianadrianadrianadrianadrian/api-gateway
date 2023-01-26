namespace Gateway.Core;

using Bearded.Monads;
using System.Net.Http;

public static class HttpRequestHelpers
{
    public static Option<string> ServiceName(this HttpRequestMessage request)
        => from uri in request.RequestUri.AsOption()
           from name in uri.Segments.FirstOrNone(s => s != "/")
           select name.Replace("/", string.Empty);

    public static Option<string> InboundRoute(this HttpRequestMessage request)
        => from uri in request.RequestUri.AsOption()
           select uri.AbsolutePath;

    public static Option<string> Query(this HttpRequestMessage request)
        => from uri in request.RequestUri.AsOption()
           from _ in (uri.Query != string.Empty || uri.Query != null).AsOption()
           select uri.Query;
}