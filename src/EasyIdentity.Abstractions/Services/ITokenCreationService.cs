using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services
{
    public interface ITokenCreationService
    {
        Task<string> CreateTokenAsync(TokenDescriptor tokenDescriptor);
        Task<string> CreateRefreshTokenAsync(TokenDescriptor tokenDescriptor);
    }
}
