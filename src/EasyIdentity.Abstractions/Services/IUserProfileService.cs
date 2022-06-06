using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services
{
    public interface IUserProfileService
    {
        Task<UserProfileResult> GetAsync(UserProfileRequest request);
    }
}
