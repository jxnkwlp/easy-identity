namespace EasyIdentity.Models
{
    public class TokenCreationResult
    {
        public TokenDescriptor TokenDescriptor { get; }
        public string AccessToken { get; }
        public string IdentityToken { get; }
        public string RefreshToken { get; }

        public TokenCreationResult(TokenDescriptor tokenDescriptor, string accessToken, string identityToken, string refreshToken)
        {
            TokenDescriptor = tokenDescriptor;
            AccessToken = accessToken;
            IdentityToken = identityToken;
            RefreshToken = refreshToken;
        }
    }
}
