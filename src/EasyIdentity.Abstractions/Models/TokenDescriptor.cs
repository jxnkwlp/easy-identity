using System;
using System.Security.Claims;

namespace EasyIdentity.Models
{
    /// <summary>
    ///  internal class
    /// </summary>
    public class TokenDescriptor
    {
        public TokenDescriptor(string subjectId, Client client)
        {
            SubjectId = subjectId;
            Client = client;
        }

        public string SubjectId { get; }

        public string TokenType { get; set; }

        public string TokenName { get; set; } = "AccessToken";

        public string Issuer { get; set; }

        public string Audiences { get; set; }

        public DateTime CreationTime { get; set; }

        public int Lifetime { get; set; }

        public Client Client { get; }

        public ClaimsIdentity Identity { get; set; }

    }
}
