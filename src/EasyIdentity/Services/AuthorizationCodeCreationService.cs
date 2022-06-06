﻿using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EasyIdentity.Services
{
    public class AuthorizationCodeCreationService : IAuthorizationCodeCreationService
    {
        public Task<string> CreateAsync(ClaimsPrincipal principal)
        {
            string code = Guid.NewGuid().ToString("N");

            return Task.FromResult(code);
        }
    }
}