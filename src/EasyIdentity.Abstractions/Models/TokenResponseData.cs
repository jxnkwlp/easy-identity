using System.Collections.Generic;

namespace EasyIdentity.Models
{
    public class TokenResponseData
    {
        public string AccessToken { get; set; }
        public string IdType { get; set; }

        public string TokenType { get; set; }

        public int ExpiresIn { get; set; }

        public string RefreshToken { get; set; }

        public string Scope { get; set; }

        public Dictionary<string, object> ExtraData { get; set; }
    }
}
