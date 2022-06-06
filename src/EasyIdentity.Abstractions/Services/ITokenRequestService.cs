using System.Threading.Tasks;

namespace EasyIdentity.Services
{
    public interface ITokenRequestService
    {
        Task HandleAsync();
    }
}
