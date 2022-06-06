using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services
{
    public class DeviceCodeRequestValidator : IDeviceCodeRequestValidator
    {
        public Task<RequestValidationResult> ValidateAsync(RequestData data)
        {
            throw new NotImplementedException();
        }
    }
}
