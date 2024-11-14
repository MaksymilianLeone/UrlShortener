using UrlShortener.Models;

namespace UrlShortener.Services.UrlService
{
    public interface IUrlService
    {
        Task<UrlShortened> CreateShortUrl(string originalUrl, string customId = null, TimeSpan? ttl = null);
        Task<string> GetOriginalUrl(string id);
        Task DeleteShortUrl(string id);
    }
}
