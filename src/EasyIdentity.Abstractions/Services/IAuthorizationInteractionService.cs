using System.Security.Claims;
using System.Threading.Tasks;

namespace EasyIdentity.Services
{
    /// <summary>
    ///  Authorization interaction service
    /// </summary>
    public interface IAuthorizationInteractionService
    {
        /// <summary>
        ///  grant or reject when input user code on device code flow 
        /// </summary> 
        /// <returns>the action success or failed</returns>
        Task<bool> DeviceUserCodeAuthorizationAsync(string userCode, ClaimsPrincipal claimsPrincipal, bool grant);
    }
}
