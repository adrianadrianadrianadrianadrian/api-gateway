namespace Gateway.Host;

using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Primitives;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

public static class HttpExt
{
    public static HttpRequestMessage HttpRequestMessage(this HttpRequest request)
    {
        var uri = new Uri(request.GetEncodedUrl());
        var message = new HttpRequestMessage(new HttpMethod(request.Method), uri);

        if (!HttpMethods.IsGet(request.Method) &&
            !HttpMethods.IsHead(request.Method) &&
            !HttpMethods.IsDelete(request.Method) &&
            !HttpMethods.IsTrace(request.Method))
        {
            message.Content = new StreamContent(request.Body);
        }

        foreach (var header in request.Headers)
        {
            if (!message.Headers.TryAddWithoutValidation(header.Key, header.Value.AsEnumerable()) && message.Content != null)
            {
                message.Content.Headers.TryAddWithoutValidation(header.Key, header.Value.AsEnumerable());
            }
        }

        return message;
    }

    public static async Task<HttpResponse> Enrich(this HttpResponse response, HttpResponseMessage message)
    {
        response.StatusCode = (int)message.StatusCode;
        response.Body = await message.Content.ReadAsStreamAsync();
        foreach (var (k, v) in message.Headers)
        {
            response.Headers.Add(k, new StringValues(v.ToArray()));
        }

        return response;
    }

    public static void RemovePathPrefix(this HttpRequest request, string pathPrefix)
    {
        if (request.Path.StartsWithSegments(new PathString($"/{pathPrefix}"), out var rest))
            request.Path = rest;
    }
}