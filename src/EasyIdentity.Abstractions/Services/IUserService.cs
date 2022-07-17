using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services
{
    /// <summary>
    ///  User service 
    /// </summary>
    public interface IUserService
    {
        Task<UserProfileResult> GetProfileAsync(UserProfileRequest request);

        Task<string> GetSubjectAsync(string username, string password, RequestData requestData);
    }
}
