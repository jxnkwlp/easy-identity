using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services
{
    public interface IClientSecretValidator
    {
        Task<bool> ValidateAsync(Client client, string secret);
    }
}
