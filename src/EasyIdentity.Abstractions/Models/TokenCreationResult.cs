namespace EasyIdentity.Models;

public class TokenCreationResult
{
    public TokenCreationResult(TokenDescriptor accessTokenDescriptor, string accessToken, TokenDescriptor identityTokenDescriptor, string identityToken, TokenDescriptor refreshTokenDescriptor, string refreshToken)
    {
        AccessTokenDescriptor = accessTokenDescriptor;
        AccessToken = accessToken;
        IdentityTokenDescriptor = identityTokenDescriptor;
        IdentityToken = identityToken;
        RefreshTokenDescriptor = refreshTokenDescriptor;
        RefreshToken = refreshToken;
    }

    public TokenDescriptor AccessTokenDescriptor { get; }
    public string AccessToken { get; }

    public TokenDescriptor IdentityTokenDescriptor { get; }
    public string IdentityToken { get; }

    public TokenDescriptor RefreshTokenDescriptor { get; }
    public string RefreshToken { get; }

}
