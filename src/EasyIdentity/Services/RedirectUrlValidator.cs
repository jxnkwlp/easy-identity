using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public class RedirectUrlValidator : IRedirectUrlValidator
{
    public Task<bool> ValidateAsync(Client client, string url)
    {
        return Task.FromResult(true);
    }
}
