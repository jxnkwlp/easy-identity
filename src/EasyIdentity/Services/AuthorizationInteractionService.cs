using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services
{
    public class AuthorizationInteractionService : IAuthorizationInteractionService
    {
        public Task<DeviceCodeAuthorizationResult> DeviceCodeAuthorizationAsync(string userCode)
        {
            throw new NotImplementedException();
        }
    }
}
