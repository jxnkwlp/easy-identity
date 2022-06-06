using System;
using System.Collections.Concurrent;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EasyIdentity.Services
{
    public class AuthorizationCodeStoreService : IAuthorizationCodeStoreService
    {
        private static readonly ConcurrentDictionary<string, AuthorizationCode> _cache = new ConcurrentDictionary<string, AuthorizationCode>();

        public Task<string> GetSubjectAsync(string code)
        {
            if (_cache.TryGetValue(code, out var subject))
            {
                if (subject.Expiration < DateTime.UtcNow)
                    return Task.FromResult(String.Empty);
                else
                    return Task.FromResult(subject.Subject);
            }

            return Task.FromResult(String.Empty);
        }

        public Task RemoveAsync(string code)
        {
            _cache.TryRemove(code, out var _);

            return Task.CompletedTask;
        }

        public Task SaveAsync(ClaimsPrincipal principal, string code, DateTime expiration)
        {
            var subject = principal.Identity.Name;

            _cache[code] = new AuthorizationCode { Subject = subject, Expiration = expiration };

            return Task.CompletedTask;
        }

        public class AuthorizationCode
        {
            public string Subject { get; set; }
            public DateTime Expiration { get; set; }
        }
    }
}
