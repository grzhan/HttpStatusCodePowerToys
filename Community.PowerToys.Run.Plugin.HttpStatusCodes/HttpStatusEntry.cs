using System.Collections.Generic;

namespace Community.PowerToys.Run.Plugin.HttpStatusCodes;

public class HttpStatusEntry(string code, string reasonPhrase, string oneLiner, string definedIn)
{
    public string Code { get; } = code;
    public string ReasonPhrase { get; } = reasonPhrase;
    public string OneLiner { get; } = oneLiner;
    public string DefinedIn { get; } = definedIn;
}

public static class HttpStatusCatalog
{
    private static readonly Dictionary<string, HttpStatusEntry> Entries = new()
    {
       {"100", new HttpStatusEntry("100", "Continue", "The server has received the request headers, and that the client should proceed to send the request body", "http://tools.ietf.org/html/rfc7231#section-6.2.1")},
       {"101", new HttpStatusEntry("101", "Switching Protocols", "The requester has asked the server to switch protocols and the server is acknowledging that it will do so", "http://tools.ietf.org/html/rfc7231#section-6.2.2")},
       {"200", new HttpStatusEntry("200", "OK", "Standard response for successful HTTP requests", "http://tools.ietf.org/html/rfc7231#section-6.3.1")},
       {"201", new HttpStatusEntry("201", "Created", "The request has been fulfilled and resulted in a new resource being created", "http://tools.ietf.org/html/rfc7231#section-6.3.2")},
       {"204", new HttpStatusEntry("204", "No Content", "The server has successfully fulfilled the request and that there is no additional content to send in the response payload body", "http://tools.ietf.org/html/rfc7231#section-6.3.5")},
       {"206", new HttpStatusEntry("206", "Partial Content", "The server is successfully fulfilling a range request for the target resource", "http://tools.ietf.org/html/rfc7233#section-4.1")},
       {"301", new HttpStatusEntry("301", "Moved Permanently", "The resource has been moved permanently to a different URI", "http://tools.ietf.org/html/rfc7231#section-6.4.2")},
       {"302", new HttpStatusEntry("302", "Found", "The server is redirecting to a different URI, as indicated by the Location header", "http://tools.ietf.org/html/rfc7231#section-6.4.3")},
       {"303", new HttpStatusEntry("303", "See Other", "The server is redirecting to a different URI which accesses the same resource", "http://tools.ietf.org/html/rfc7231#section-6.4.4")},
       {"304", new HttpStatusEntry("304", "Not Modified", "There is no need to retransmit the resource, since the client still has a previously-downloaded copy", "http://tools.ietf.org/html/rfc7232#section-4.1")},
       {"305", new HttpStatusEntry("305", "Use Proxy", "The requested resource is only available through a proxy, whose address is provided in the response", "http://tools.ietf.org/html/rfc7231#section-6.4.5")},
       {"307", new HttpStatusEntry("307", "Temporary Redirect", "Subsequent requests should use the specified proxy", "http://tools.ietf.org/html/rfc7231#section-6.4.7")},
       {"400", new HttpStatusEntry("400", "Bad Request", "The request could not be understood by the server due to malformed syntax", "http://tools.ietf.org/html/rfc7231#section-6.5.1")},
       {"401", new HttpStatusEntry("401", "Unauthorized", "Authentication is required and has failed or has not yet been provided", "http://tools.ietf.org/html/rfc7235#section-3.1")},
       {"402", new HttpStatusEntry("402", "Payment Required", "The 402 (Payment Required) status code is reserved for future use", "http://tools.ietf.org/html/rfc7231#section-6.5.2")},
       {"403", new HttpStatusEntry("403", "Forbidden", "The server understood the request but refuses to authorize it", "http://tools.ietf.org/html/rfc7231#section-6.5.3")},
       {"404", new HttpStatusEntry("404", "Not Found", "The requested resource could not be found but may be available again in the future", "http://tools.ietf.org/html/rfc7231#section-6.5.4")},
       {"405", new HttpStatusEntry("405", "Method Not Allowed", "A request was made of a resource using a request method not supported by that resource", "http://tools.ietf.org/html/rfc7231#section-6.5.5")},
       {"406", new HttpStatusEntry("406", "Not Acceptable", "The requested resource is only capable of generating content not acceptable according to the Accept headers sent in the request", "http://tools.ietf.org/html/rfc7231#section-6.5.6")},
       {"407", new HttpStatusEntry("407", "Proxy Authentication Required", "The client must first authenticate itself with the proxy", "http://tools.ietf.org/html/rfc7235#section-3.2")},
       {"408", new HttpStatusEntry("408", "Request Timeout", "The server timed out waiting for the request", "http://tools.ietf.org/html/rfc7231#section-6.5.7")},
       {"409", new HttpStatusEntry("409", "Conflict", "The request could not be processed because of conflict in the request", "http://tools.ietf.org/html/rfc7231#section-6.5.8")},
       {"410", new HttpStatusEntry("410", "Gone", "The resource requested is no longer available and will not be available again", "http://tools.ietf.org/html/rfc7231#section-6.5.9")},
       {"411", new HttpStatusEntry("411", "Length Required", "The request did not specify the length of its content, which is required by the requested resource", "http://tools.ietf.org/html/rfc7231#section-6.5.10")},
       {"412", new HttpStatusEntry("412", "Precondition Failed", "The server does not meet one of the preconditions that the requester put on the request", "http://tools.ietf.org/html/rfc7232#section-4.2")},
       {"413", new HttpStatusEntry("413", "Payload Too Large", "The request is larger than the server is willing or able to process", "http://tools.ietf.org/html/rfc7231#section-6.5.11")},
       {"414", new HttpStatusEntry("414", "URI Too Long", "The URI provided was too long for the server to process", "http://tools.ietf.org/html/rfc7231#section-6.5.12")},
       {"415", new HttpStatusEntry("415", "Unsupported Media Type", "The request entity has a media type which the server or resource does not support", "http://tools.ietf.org/html/rfc7231#section-6.5.13")},
       {"416", new HttpStatusEntry("416", "Range Not Satisfiable", "The client has asked for a portion of the file (byte serving), but the server cannot supply that portion", "http://tools.ietf.org/html/rfc7233#section-4.4")},
       {"417", new HttpStatusEntry("417", "Expectation Failed", "The server cannot meet the requirements of the Expect request-header field", "http://tools.ietf.org/html/rfc7231#section-6.5.14")},
       {"426", new HttpStatusEntry("426", "Upgrade Required", "The client should switch to a different protocol", "http://tools.ietf.org/html/rfc7231#section-6.5.15")},
       {"500", new HttpStatusEntry("500", "Internal Server Error", "The server encountered an unexpected condition that prevented it from fulfilling the request", "http://tools.ietf.org/html/rfc7231#section-6.6.1")},
       {"501", new HttpStatusEntry("501", "Not Implemented", "The server does not support the functionality required to fulfill the request", "http://tools.ietf.org/html/rfc7231#section-6.6.2")},
       {"502", new HttpStatusEntry("502", "Bad Gateway", "The server, while acting as a gateway or proxy, received an invalid response from an inbound server it accessed while attempting to fulfill the request", "http://tools.ietf.org/html/rfc7231#section-6.6.3")},
       {"503", new HttpStatusEntry("503", "Service Unavailable", "The server is currently unable to handle the request due to a temporary overload or scheduled maintenance", "http://tools.ietf.org/html/rfc7231#section-6.6.4")},
       {"504", new HttpStatusEntry("504", "Gateway Timeout", "The server, while acting as a gateway or proxy, did not receive a timely response from an upstream server", "http://tools.ietf.org/html/rfc7231#section-6.6.5")},
       {"505", new HttpStatusEntry("505", "HTTP Version Not Supported", "The server does not support, or refuses to support, the major version of HTTP that was used in the request", "http://tools.ietf.org/html/rfc7231#section-6.6.6")},
    };
    
    /// <summary>
    ///  Try to find an entry by its code.
    /// </summary>
    public static bool TryFindByCode(string code, out HttpStatusEntry? entry)
    {
        return Entries.TryGetValue(code, out entry);
    }
}