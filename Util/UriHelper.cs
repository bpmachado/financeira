using Microsoft.AspNetCore.Http;

namespace financeira.Util
{
    public static class UriHelper
    {
        public static Uri GerarHeaderLocation(HttpRequest request, Guid id)
        {
            var uriBuilder = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Port = request.Host.Port ?? (request.Scheme == "https" ? 443 : 80),
                Path = $"{request.Path}/{id}"
            };

            return uriBuilder.Uri;
        }
    }
}
