using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EasyIdentity.Services
{
    public interface IAuthorizationCodeCreationService
    {
        Task<string> CreateAsync(ClaimsPrincipal principal);
    }
}
