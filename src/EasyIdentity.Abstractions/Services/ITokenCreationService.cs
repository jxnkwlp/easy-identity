using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public interface ITokenCreationService
{
    Task<string> CreateAccessTokenAsync(TokenDescriptor tokenDescriptor);

    Task<string> CreateRefreshTokenAsync(TokenDescriptor tokenDescriptor, string accessToken);

    Task<string> CreateIdentityTokenAsync(TokenDescriptor tokenDescriptor, string accessToken);
}
