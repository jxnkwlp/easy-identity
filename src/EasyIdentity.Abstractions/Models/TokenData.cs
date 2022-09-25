using System.Collections.Generic;

namespace EasyIdentity.Models;

public class TokenData
{
    public string AccessToken { get; set; }
    public int ExpiresIn { get; set; }
    public string TokenType { get; set; }

    public string IdToken { get; set; }
    public int? IdTokenExpiresIn { get; set; }

    public string RefreshToken { get; set; }
    public int? RefreshTokenExpiresIn { get; set; }

    public string Scope { get; set; }

    public Dictionary<string, object> ExtraData { get; set; }

    public Dictionary<string, object> ToDictionary()
    {
        var data = new Dictionary<string, object>
        {
            ["access_token"] = AccessToken,
            ["expires_in"] = ExpiresIn
        };

        if (!string.IsNullOrEmpty(TokenType))
            data["token_type"] = TokenType;

        if (!string.IsNullOrEmpty(RefreshToken))
            data["refresh_token"] = RefreshToken;
        if (RefreshTokenExpiresIn.HasValue)
            data["refresh_expires_in"] = RefreshTokenExpiresIn;

        if (!string.IsNullOrEmpty(IdToken))
            data["id_token"] = IdToken;
        if (IdTokenExpiresIn.HasValue)
            data["id_expires_in"] = IdTokenExpiresIn;

        if (!string.IsNullOrEmpty(Scope))
            data["scope"] = Scope;

        if (ExtraData != null)
        {
            foreach (var item in ExtraData)
            {
                data[item.Key] = item.Value;
            }
        }

        return data;
    }
}
