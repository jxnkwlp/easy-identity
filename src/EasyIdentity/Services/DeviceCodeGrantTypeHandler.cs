using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services
{
    public class DeviceCodeGrantTypeHandler : IGrantTypeHandler
    {
        public string GrantType => GrantTypesConsts.DeviceCode;

        public Task<GrantTypeHandleResult> HandleAsync(GrantTypeHandleRequest context)
        {
            throw new NotImplementedException();
        }
    }
}
