using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Models;

namespace UrlShortener.Services.UrlService
{
    public class UrlService : IUrlService
    {
        private readonly UrlContext _context;

        public UrlService(UrlContext context)
        {
            _context = context;
        }

        public async Task<UrlShortened> CreateShortUrl(string originalUrl, string customId = null, TimeSpan? ttl = null)
        {
            if (string.IsNullOrEmpty(originalUrl))
            {
                throw new ArgumentException("Original URL cannot be null or empty.");
            }

            if (originalUrl.Length > 2048)  
            {
                throw new ArgumentException("The URL is too long.");
            }
            if (!Uri.IsWellFormedUriString(originalUrl, UriKind.Absolute))
            {
                throw new UriFormatException("The URL is not in a valid format.");
            }

            var id = customId ?? Guid.NewGuid().ToString();

            if (await _context.UrlShortened.AnyAsync(u => u.Id == id))
            {
                throw new Exception("The ID is already in use.");
            }

            var expirationDate = ttl.HasValue ? DateTime.UtcNow.Add(ttl.Value) : (DateTime?)null;

            var shortUrl = new UrlShortened
            {
                Id = id,  
                OriginalUrl = originalUrl,
                ExpirationDate = expirationDate,
            };

            _context.UrlShortened.Add(shortUrl);
            await _context.SaveChangesAsync();

            return shortUrl;
        }

        public async Task DeleteShortUrl(string id)
        {
            var shortUrl = await _context.UrlShortened.FindAsync(id);

            if (shortUrl != null)
            {
                _context.UrlShortened.Remove(shortUrl);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<string> GetOriginalUrl(string id)
        {
            var shortUrl = await _context.UrlShortened.FindAsync(id);
            if (shortUrl == null || shortUrl.ExpirationDate.HasValue && shortUrl.ExpirationDate < DateTime.UtcNow)
                throw new KeyNotFoundException("Url not found or expired");

            return shortUrl.OriginalUrl;
        }

        private string GenerateShortId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
