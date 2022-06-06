using System.Collections.Generic;

namespace EasyIdentity.Models
{
    public class UserProfileRequest
    {
        public UserProfileRequest(Dictionary<string, string> rawData, Client client, string userName, string password)
        {
            RawData = rawData;
            Client = client;
            UserName = userName;
            Password = password;
        }

        public UserProfileRequest(Dictionary<string, string> rawData, Client client, string subject)
        {
            RawData = rawData;
            Client = client;
            Subject = subject;
        }

        public Dictionary<string, string> RawData { get; protected set; }

        public Client Client { get; }

        public string UserName { get; }

        public string Password { get; }

        public string Subject { get;  }
    }
}
