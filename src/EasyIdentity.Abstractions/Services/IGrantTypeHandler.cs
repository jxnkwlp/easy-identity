using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services
{
    public interface IGrantTypeHandler
    {
        string GrantType { get; }

        Task<GrantTypeHandleResult> HandleAsync(GrantTypeHandleRequest context);
    }
}
