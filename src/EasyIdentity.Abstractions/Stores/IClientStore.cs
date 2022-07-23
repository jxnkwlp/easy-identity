using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Stores;

public interface IClientStore
{
    Task<Client> FindClientAsync(string clientId);
}
