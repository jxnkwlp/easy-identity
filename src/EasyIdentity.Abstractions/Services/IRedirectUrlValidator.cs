using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services
{
    public interface IRedirectUrlValidator
    {
        Task<bool> ValidateAsync(Client client, string url);
    }
}
