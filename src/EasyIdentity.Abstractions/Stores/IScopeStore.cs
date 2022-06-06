using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Stores
{
    public interface IScopeStore
    {
        Task<Scope> FindScopeAsync(string name);
    }
}
