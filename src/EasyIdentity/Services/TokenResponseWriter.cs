using System.Collections.Generic;
using System.Threading.Tasks;
using EasyIdentity.Models;
using Microsoft.AspNetCore.Http;

namespace EasyIdentity.Services
{
    public class TokenResponseWriter : ITokenResponseWriter
    {
        public async Task WriteAsync(HttpContext context, GrantTypeHandleResult result)
        {
            var data = new Dictionary<string, object>();

            if (result.HasError)
            {
                data["error"] = result.Error;
                data["error_description"] = result.ErrorDescription;
            }
            else
            {
                data["access_token"] = result.ResponseData.AccessToken;
                data["expires_in"] = result.ResponseData.ExpiresIn;
                if (!string.IsNullOrEmpty(result.ResponseData.TokenType))
                    data["token_type"] = result.ResponseData.TokenType;
                if (!string.IsNullOrEmpty(result.ResponseData.RefreshToken))
                    data["refresh_token"] = result.ResponseData.RefreshToken;
                if (!string.IsNullOrEmpty(result.ResponseData.Scope))
                    data["scope"] = result.ResponseData.Scope;
                if (!string.IsNullOrEmpty(result.ResponseData.IdType))
                    data["id_token"] = result.ResponseData.IdType;

                if (result.ResponseData.ExtraData != null)
                {
                    foreach (var item in result.ResponseData.ExtraData)
                    {
                        data[item.Key] = item.Value;
                    }
                }

            }

#if NET6_0_OR_GREATER
            context.Response.Headers.CacheControl = "no-store";
#elif NET5_0
            context.Response.Headers["Cache-Control"] = "no-store";
#endif

            if (result.GrantType == GrantTypesConsts.Implicit)
            {
                var redirect_uri = string.Empty;
                context.Response.Redirect($"{redirect_uri}?access_token={data["access_token"]}&token_type=Bearer&expires_in={data["expires_in"]}&scope={data["scope"]}&state={data["state"]}");
            }
            else
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(data);
            }
        }
    }
}
